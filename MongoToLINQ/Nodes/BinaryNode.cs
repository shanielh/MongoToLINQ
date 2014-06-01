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

            _expression = System.Linq.Expressions.Expression.Lambda<Func<T, bool>>(
                System.Linq.Expressions.Expression.MakeBinary(expressionType, left, System.Linq.Expressions.Expression.Constant(convertedValue)), argument);
        }

        public Expression<Func<T, bool>> Expression
        {
            get
            {
                return _expression;
            }

        }
    }
}