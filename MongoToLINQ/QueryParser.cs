using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using CodeSharp.MongoToLINQ.Nodes;
using Newtonsoft.Json.Linq;

namespace CodeSharp.MongoToLINQ
{
    public class QueryParser : IQueryParser
    {
        private readonly ITypeDescriptorFactory _typeDescriptorFactory;
        private readonly ParameterExpression _argument;

        public QueryParser(Type argumentType, ITypeDescriptorFactory typeDescriptorFactory)
        {
            _typeDescriptorFactory = typeDescriptorFactory;
            _argument = Expression.Parameter(argumentType);
        }

        public Expression ParseWhere(JToken query)
        {
            var queryObject = (JObject)query;
            var delegateType = typeof (Func<,>).MakeGenericType(_argument.Type, typeof (bool));
            var result = Parse(_argument, queryObject).Result;

            return Expression.Lambda(delegateType, result, _argument);
        }

        private IQueryNode Parse(Expression path, JObject query)
        {
            return new AndNode(query.OfType<JProperty>().Select(property => CreateNode(path, property)));
        }

        private IQueryNode CreateNode(Expression path, JProperty property)
        {
            if (property.Name[0] == '$' && property.Name != "$size")
            {
                // Special functions.
                if (property.Name == "$or")
                {
                    var array = (JArray)property.Value;
                    return new OrNode(array.Values<JObject>().Select(o => Parse(path, o)));
                }

                if (property.Name == "$nor")
                {
                    var array = (JArray)property.Value;
                    return new NorNode(array.Values<JObject>().Select(o => Parse(path, o)));
                }

                if (property.Name == "$not")
                {
                    return new NotNode(Parse(path, (JObject)property.Value));
                }

                if (property.Name == "$in")
                {
                    var array = (JArray)property.Value;

                    return new InNode(path, array);
                }

                if (property.Name == "$elemMatch")
                {
                    var parser = new QueryParser(GetEnumerableType(path), _typeDescriptorFactory);

                    var parsedElement = parser.GetType()
                        .GetMethod("ParseWhere")
                        .Invoke(parser, new object[] { property.Value }) as Expression;

                    return new ElementMatchNode(path, parsedElement);
                }

                if (property.Name == "$nin")
                {
                    var array = (JArray)property.Value;

                    return new NotNode(new InNode(path, array));
                }

                if (BinaryNode.Arguments.ContainsKey(property.Name))
                {
                    return new BinaryNode(path, property);
                }

                throw new NotSupportedException(string.Format("{0} Does not support parsing element type {1}", GetType(),
                    property.Name));
            }

            Expression left = property.Name == "$size"
                ? SizeNode.GetExpression(_argument, path)
                : _typeDescriptorFactory.Create(path.Type).GetStateProperty(property.Name).CreateGetExpression(path);

            if (property.HasValues && property.Value is JObject)
            {
                // Composite object
                IEnumerable<IQueryNode> innerNodes = property.Values<JObject>().Select(o => Parse(left, o));
                if (left.Type.IsClass)
                {
                    // Check nullability.
                    var extraNode = new BinaryNode(left, new JProperty("$ne", null));
                    innerNodes = new[] { extraNode }.Concat(innerNodes);
                }
                return new AndNode(innerNodes);
            }

            return new EqualQueryNode(left, ((JValue)property.Value).Value);
        }

        internal static Type GetEnumerableType(Expression path)
        {
            return
                path.Type.GetInterfaces()
                    .Single(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof (IEnumerable<>))
                    .GetGenericArguments()[0];
        }
    }

    public class QueryParser<T> : IQueryParser<T>
    {
        private readonly IQueryParser _parser;

        public QueryParser(ITypeDescriptorFactory typeDescriptor)
        {
            _parser = new QueryParser(typeof (T), typeDescriptor);
        }

        public Expression<Func<T, bool>> ParseWhere(JToken query)
        {
            return (Expression<Func<T, bool>>) _parser.ParseWhere(query);
        }

    }

    internal static class SizeNode
    {
        
        public static Expression GetExpression(ParameterExpression argument, Expression path)
        {
            var method =
                typeof (Enumerable).GetMethods()
                    .Single(mi => mi.Name == "Count" && mi.GetParameters().Length == 1)
                    .MakeGenericMethod(QueryParser.GetEnumerableType(path));

            return Expression.Call(method, path);
        }

    }
}