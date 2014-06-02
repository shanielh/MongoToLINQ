using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using CodeSharp.MongoToLINQ.Nodes;
using Newtonsoft.Json.Linq;

namespace CodeSharp.MongoToLINQ
{
    public class QueryParser<T> : IQueryParser<T>
    {
        private readonly ParameterExpression _argument;

        public QueryParser()
        {
            _argument = Expression.Parameter(typeof(T));
        }

        public Expression<Func<T, bool>> ParseWhere(JToken query)
        {
            var queryObject = (JObject)query;

            return Parse(_argument, queryObject).Expression;

        }

        private IQueryNode<T> Parse(Expression path, JObject query)
        {
            return new AndNode<T>(_argument,
                query.OfType<JProperty>().Select(property => CreateNode(path, property)).ToList());
        }

        private IQueryNode<T> CreateNode(Expression path, JProperty property)
        {
            if (property.Name[0] == '$' && property.Name != "$size")
            {
                // Special functions.
                if (property.Name == "$or")
                {
                    var array = (JArray)property.Value;
                    return new OrNode<T>(_argument, array.Values<JObject>().Select(o => Parse(path, o)).ToList());
                }

                if (property.Name == "$nor")
                {
                    var array = (JArray)property.Value;
                    return new NorNode<T>(_argument, array.Values<JObject>().Select(o => Parse(path, o)).ToList());
                }

                if (property.Name == "$not")
                {
                    return new NotNode<T>(_argument, Parse(path, (JObject)property.Value));
                }

                if (property.Name == "$in")
                {
                    var array = (JArray)property.Value;

                    return new InNode<T>(_argument, path, array);
                }

                if (property.Name == "$elemMatch")
                {
                    var parser =
                        typeof(QueryParser<>).MakeGenericType(GetEnumerableType(path)).GetConstructor(Type.EmptyTypes).Invoke(null);

                    var parsedElement = parser.GetType()
                        .GetMethod("ParseWhere")
                        .Invoke(parser, new object[] { property.Value }) as Expression;


                    return new ElementMatchNode<T>(_argument, path, parsedElement);
                }

                if (property.Name == "$nin")
                {
                    var array = (JArray)property.Value;

                    return new NotNode<T>(_argument, new InNode<T>(_argument, path, array));
                }

                if (BinaryNodeHelper.Arguments.ContainsKey(property.Name))
                {
                    return new BinaryNode<T>(_argument, path, property);
                }

                throw new NotSupportedException();
            }

            Expression left = property.Name == "$size" ? 
                SizeNode<T>.GetExpression(_argument, path) : 
                Expression.Property(path, property.Name);

            if (property.HasValues && property.Value is JObject)
            {
                // Composite object
                IEnumerable<IQueryNode<T>> innerNodes = property.Values<JObject>().Select(o => Parse(left, o));
                if (left.Type.IsClass)
                {
                    // Check nullability.
                    var extraNode = new BinaryNode<T>(_argument, left, new JProperty("$ne", null));
                    innerNodes = new[] { extraNode }.Concat(innerNodes);
                }
                return new AndNode<T>(_argument, innerNodes.ToList());
            }

            return new EqualQueryNode<T>(_argument, left, ((JValue)property.Value).Value);
        }

        internal static Type GetEnumerableType(Expression path)
        {
            return path.Type.GetInterfaces().Single(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>)).GetGenericArguments()[0];
        }
    }

    internal static class SizeNode<T>
    {
        
        public static Expression GetExpression(ParameterExpression argument, Expression path)
        {
            var method =
                typeof (Enumerable).GetMethods()
                    .Single(mi => mi.Name == "Count" && mi.GetParameters().Length == 1)
                    .MakeGenericMethod(QueryParser<T>.GetEnumerableType(path));

            return Expression.Call(method, path);
        }

    }
}