using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CodeSharp.MongoToLINQ.Nodes
{
    internal class OrNode<T> : IQueryNode<T>
    {
        private readonly Expression<Func<T, bool>> _expression;

        public OrNode(ParameterExpression argument, IEnumerable<IQueryNode<T>> innerNodes)
        {
            Expression expression =
                innerNodes.Select(n => n.Expression.Body)
                    .Aggregate(System.Linq.Expressions.Expression.OrElse);

            _expression = System.Linq.Expressions.Expression.Lambda<Func<T, bool>>(expression, argument);
        }

        public Expression<Func<T, bool>> Expression
        {
            get
            {
                return _expression;
            }
        }
    }
}