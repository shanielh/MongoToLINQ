using System.Collections.Generic;
using System.Linq.Expressions;

namespace CodeSharp.MongoToLINQ.Nodes
{
    internal class NorNode : IQueryNode
    {
        private readonly Expression _expression;

        public NorNode(IEnumerable<IQueryNode> innerNodes)
        {
            _expression = Expression.Constant(true);

            foreach (var node in innerNodes)
            {
                _expression = Expression.MakeBinary(ExpressionType.AndAlso, _expression,
                    Expression.Negate(node.Result));
            }
        }

        public Expression Result
        {
            get { return _expression; }
        }
    }
}