using System;
using System.Linq;
using System.Linq.Expressions;
using Newtonsoft.Json.Linq;

namespace CodeSharp.MongoToLINQ.Nodes
{
    internal class InNode<T> : IQueryNode<T>
    {
        private readonly Expression<Func<T, bool>> _expression;

        public InNode(ParameterExpression argument, Expression path, JArray array)
        {
            Array objects = ConvertArray(array, path.Type);

            var method = typeof(Enumerable).GetMethods().Single(mi => mi.Name == "Contains" && mi.GetParameters().Length == 2).MakeGenericMethod(path.Type);

            _expression = Expression.Lambda<Func<T, bool>>(
                Expression.Call(method, Expression.Constant(objects), path), argument);
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

        public Expression<Func<T, bool>> GetExpression
        {
            get
            {
                return _expression;
            }
        }
    }
}