using System;
using System.Linq;
using System.Linq.Expressions;

namespace CodeSharp.MongoToLINQ.Nodes
{
    internal class ElementMatchNode<T> : IQueryNode<T>
    {
        private readonly Expression<Func<T, bool>> _expression;

        public ElementMatchNode(ParameterExpression argument, Expression path, Expression innerExpression)
        {
            var method =
                typeof (Enumerable).GetMethods()
                    .Single(mi => mi.Name == "Any" && mi.GetParameters().Length == 2)
                    .MakeGenericMethod(QueryParser<T>.GetEnumerableType(path));

            _expression = System.Linq.Expressions.Expression.Lambda<Func<T, bool>>(System.Linq.Expressions.Expression.Call(method, path, innerExpression), argument);
        }


        public Expression<Func<T, bool>> Expression
        {
            get { return _expression; }
        }
    }
}