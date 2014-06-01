using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CodeSharp.MongoToLINQ.Nodes
{
    internal class NorNode<T> : IQueryNode<T>
    {
        private readonly Expression<Func<T, bool>> _expression;

        public NorNode(ParameterExpression argument, IEnumerable<IQueryNode<T>> innerNodes)
        {
            Expression expression = Expression.Constant(true);

            foreach (var node in innerNodes)
            {
                expression = Expression.MakeBinary(ExpressionType.AndAlso, expression,
                    Expression.Negate(node.GetExpression.Body));
            }

            _expression = Expression.Lambda<Func<T, bool>>(expression, argument);
        }

        public Expression<Func<T, bool>> GetExpression
        {
            get { return _expression; }
        }
    }
}