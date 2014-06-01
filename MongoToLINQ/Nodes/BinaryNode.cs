using System;
using System.Linq.Expressions;
using Newtonsoft.Json.Linq;

namespace CodeSharp.MongoToLINQ.Nodes
{
    internal class BinaryNode<T> : IQueryNode<T>
    {

        private readonly Expression<Func<T, bool>> _expression;

        public BinaryNode(ParameterExpression argument, Expression left, JProperty property)
        {
            var convertedValue = QueryNodeHelper.ConvertValue(left, property.Value);

            var expressionType = BinaryNodeHelper.Arguments[property.Name];

            _expression = Expression.Lambda<Func<T, bool>>(
                Expression.MakeBinary(expressionType, left, Expression.Constant(convertedValue)), argument);
        }

        public Expression<Func<T, bool>> GetExpression
        {
            get
            {
                return _expression;
            }

        }
    }
}