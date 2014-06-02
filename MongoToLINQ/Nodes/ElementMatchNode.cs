using System.Linq;
using System.Linq.Expressions;

namespace CodeSharp.MongoToLINQ.Nodes
{
    internal class ElementMatchNode : IQueryNode
    {
        private readonly MethodCallExpression _expression;

        public ElementMatchNode(Expression path, Expression innerExpression)
        {
            var method =
                typeof (Enumerable).GetMethods()
                    .Single(mi => mi.Name == "Any" && mi.GetParameters().Length == 2)
                    .MakeGenericMethod(QueryParser.GetEnumerableType(path));

            _expression = Expression.Call(method, path, innerExpression);
        }


        public Expression Result
        {
            get { return _expression; }
        }
    }
}