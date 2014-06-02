using System.Linq.Expressions;

namespace CodeSharp.MongoToLINQ.Nodes
{
    internal class EqualQueryNode : IQueryNode
    {
        private readonly BinaryExpression _expression;

        public EqualQueryNode(Expression path, object value)
        {
            var convertedValue = QueryNodeHelper.ConvertValue(path, value);

            _expression = 
                Expression.Equal(path, Expression.Constant(convertedValue));
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