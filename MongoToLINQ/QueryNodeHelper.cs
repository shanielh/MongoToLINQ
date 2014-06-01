using System;
using System.Linq.Expressions;

namespace CodeSharp.MongoToLINQ
{
    internal static class QueryNodeHelper
    {


        public static object ConvertValue(Expression expression, object value)
        {
            return Convert.ChangeType(value, expression.Type);
        }
    }
}