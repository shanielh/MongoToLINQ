using System;
using System.Linq.Expressions;

namespace CodeSharp.MongoToLINQ.Nodes
{
    class NotNode<T> : IQueryNode<T>
    {
        private readonly Expression<Func<T, bool>> _expression;

        public NotNode(ParameterExpression argument, IQueryNode<T> child)
        {
            _expression = Expression.Lambda<Func<T, bool>>(Expression.Not(child.GetExpression.Body), argument);
        }

        public Expression<Func<T, bool>> GetExpression
        {
            get { return _expression; }
        }
    }
}
