using System.Linq.Expressions;

namespace CodeSharp.MongoToLINQ.Nodes
{
    class NotNode : IQueryNode
    {
        private readonly UnaryExpression _expression;

        public NotNode(IQueryNode child)
        {
            _expression = Expression.Not(child.Result);
        }

        public Expression Result
        {
            get { return _expression; }
        }
    }
}
