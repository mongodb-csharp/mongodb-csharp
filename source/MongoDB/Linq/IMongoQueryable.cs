using System.Linq;

namespace MongoDB.Linq
{
    internal interface IMongoQueryable : IQueryable
    {
        IUntypedCollection Collection { get; }

        MongoQueryObject GetQueryObject();
    }
}