using System.Linq.Expressions;

namespace CodeSharp.MongoToLINQ
{
    public interface IQueryNode
    {

        Expression Result
        {
            get;
        }

    }
}