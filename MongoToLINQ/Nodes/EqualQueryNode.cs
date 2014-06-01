using System;
using System.Linq.Expressions;

namespace CodeSharp.MongoToLINQ.Nodes
{
    internal class EqualQueryNode<T> : IQueryNode<T>
    {
        private readonly Expression<Func<T, bool>> _expression;

        public EqualQueryNode(ParameterExpression argument, MemberExpression path, object value)
        {
            var convertedValue = QueryNodeHelper.ConvertValue(path, value);

            _expression = System.Linq.Expressions.Expression.Lambda<Func<T, bool>>(
                System.Linq.Expressions.Expression.Equal(path, System.Linq.Expressions.Expression.Constant(convertedValue)), argument);
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