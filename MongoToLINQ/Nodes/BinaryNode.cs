using System.Collections.Generic;
using System.Linq.Expressions;
using Newtonsoft.Json.Linq;

namespace CodeSharp.MongoToLINQ.Nodes
{
    internal class BinaryNode : IQueryNode
    {
        internal static readonly IDictionary<string, ExpressionType>
            Arguments = new Dictionary<string, ExpressionType>
            {
                {"$gt", ExpressionType.GreaterThan},
                {"$gte", ExpressionType.GreaterThanOrEqual},
                {"$lt", ExpressionType.LessThan},
                {"$lte", ExpressionType.LessThanOrEqual},
                {"$eq", ExpressionType.Equal},
                {"$ne", ExpressionType.NotEqual},
            };

        private readonly BinaryExpression _expression;

        public BinaryNode(Expression left, JProperty property)
        {
            var convertedValue = QueryNodeHelper.ConvertValue(left, property.Value);

            var expressionType = Arguments[property.Name];

            _expression = 
                Expression.MakeBinary(expressionType, left, Expression.Constant(convertedValue));
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