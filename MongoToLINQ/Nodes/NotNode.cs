using System;
using System.Linq.Expressions;

namespace CodeSharp.MongoToLINQ.Nodes
{
    class NotNode<T> : IQueryNode<T>
    {
        private readonly Expression<Func<T, bool>> _expression;

        public NotNode(ParameterExpression argument, IQueryNode<T> child)
        {
            _expression = System.Linq.Expressions.Expression.Lambda<Func<T, bool>>(System.Linq.Expressions.Expression.Not(child.Expression.Body), argument);
        }

        public Expression<Func<T, bool>> Expression
        {
            get { return _expression; }
        }
    }
}
