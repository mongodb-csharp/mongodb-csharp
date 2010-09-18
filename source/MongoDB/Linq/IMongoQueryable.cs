using System.Linq;

namespace MongoDB.Linq
{
    internal interface IMongoQueryable : IQueryable
    {
        IMongoCollection Collection { get; }

        MongoQueryObject GetQueryObject();
    }
}