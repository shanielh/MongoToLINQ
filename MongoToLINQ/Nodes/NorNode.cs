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
            Expression expression = System.Linq.Expressions.Expression.Constant(true);

            foreach (var node in innerNodes)
            {
                expression = System.Linq.Expressions.Expression.MakeBinary(ExpressionType.AndAlso, expression,
                    System.Linq.Expressions.Expression.Negate(node.Expression.Body));
            }

            _expression = System.Linq.Expressions.Expression.Lambda<Func<T, bool>>(expression, argument);
        }

        public Expression<Func<T, bool>> Expression
        {
            get { return _expression; }
        }
    }
}