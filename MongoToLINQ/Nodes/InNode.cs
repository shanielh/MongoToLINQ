using System;
using System.Linq;
using System.Linq.Expressions;
using Newtonsoft.Json.Linq;

namespace CodeSharp.MongoToLINQ.Nodes
{
    internal class InNode : IQueryNode
    {
        private readonly MethodCallExpression _expression;

        public InNode(Expression path, JArray array)
        {
            Array objects = ConvertArray(array, path.Type);

            var method = typeof(Enumerable).GetMethods().Single(mi => mi.Name == "Contains" && mi.GetParameters().Length == 2).MakeGenericMethod(path.Type);

            _expression = Expression.Call(method, Expression.Constant(objects), path);
        }

        private Array ConvertArray(JArray array, Type type)
        {
            var retVal = Array.CreateInstance(type, array.Count);

            for (int i = 0; i < array.Count; i++)
            {
                var item = array[i];
                retVal.SetValue(Convert.ChangeType(item, type), i);
            }

            return retVal;
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