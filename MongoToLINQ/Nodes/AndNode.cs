using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CodeSharp.MongoToLINQ.Nodes
{
    public class AndNode : IQueryNode
    {

        private readonly Expression _expression;

        public AndNode(IEnumerable<IQueryNode> innerNodes)
        {
            _expression = innerNodes.Select(n => n.Result).Aggregate(Expression.AndAlso);
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