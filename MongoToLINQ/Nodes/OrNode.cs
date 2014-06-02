using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CodeSharp.MongoToLINQ.Nodes
{
    internal class OrNode : IQueryNode
    {
        private readonly Expression _expression;

        public OrNode(IEnumerable<IQueryNode> innerNodes)
        {
            _expression =
                innerNodes.Select(n => n.Result)
                    .Aggregate(Expression.OrElse);
        }

        public Expression Result
        {
            get
            {
                return _expression;
            }
        }
    }
}