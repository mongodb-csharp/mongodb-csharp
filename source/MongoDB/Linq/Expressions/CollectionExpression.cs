using System;

namespace MongoDB.Linq.Expressions
{
    internal class CollectionExpression : AliasedExpression
    {
        public IMongoCollection Collection { get; private set; }

        public Type DocumentType { get; private set; }

        public CollectionExpression(Alias alias, IMongoCollection collection, Type documentType)
            : base(MongoExpressionType.Collection, typeof(void), alias)
        {
            Collection = collection;
            DocumentType = documentType;
        }
    }
}