using System;

namespace MongoDB.Linq.Expressions
{
    internal class CollectionExpression : AliasedExpression
    {
        public IUntypedCollection Collection { get; private set; }

        public Type DocumentType { get; private set; }

        public CollectionExpression(Alias alias, IUntypedCollection collection, Type documentType)
            : base(MongoExpressionType.Collection, typeof(void), alias)
        {
            Collection = collection;
            DocumentType = documentType;
        }
    }
}