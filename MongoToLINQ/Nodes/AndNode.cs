using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CodeSharp.MongoToLINQ.Nodes
{
    public class AndNode<T> : IQueryNode<T>
    {

        private readonly Expression<Func<T, bool>> _expression;

        public AndNode(ParameterExpression argument, IEnumerable<IQueryNode<T>> innerNodes)
        {
            Expression expression = System.Linq.Expressions.Expression.Constant(true);

            foreach (var node in innerNodes)
            {
                expression = System.Linq.Expressions.Expression.AndAlso(expression, node.Expression.Body);
            }

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