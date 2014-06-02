using System;
using System.Linq.Expressions;
using Newtonsoft.Json.Linq;

namespace CodeSharp.MongoToLINQ
{
    public interface IQueryParser<T>
    {
        Expression<Func<T, bool>> ParseWhere(JToken query);

    }

    public interface IQueryParser
    {
        Expression ParseWhere(JToken query);
    }
}