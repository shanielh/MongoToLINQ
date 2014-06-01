using System.Collections.Generic;
using System.Linq.Expressions;

namespace CodeSharp.MongoToLINQ.Nodes
{
    internal static class BinaryNodeHelper
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
    }
}