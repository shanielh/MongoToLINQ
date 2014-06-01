using System;
using System.Linq.Expressions;

namespace CodeSharp.MongoToLINQ
{
    public interface IQueryNode<T>
    {

        Expression<Func<T, bool>> Expression
        {
            get;
        }

    }
}