using System.Collections;

namespace MongoDB
{
    internal interface IUntypedCollection
    {
        long Count();

        long Count(object selector);

        ICursor FindAll();

        ICursor Find(object spec);

        object FindAndModify(object document, object selector, object sort, bool returnNew);

        void Insert(object document, bool safemode);

        void InsertMany(IEnumerable documents, bool safemode);

        MapReduce MapReduce();

        void Remove(object selector, bool safemode);

        void Save(object document, bool safemode);

        void Update(object document, object selector, UpdateFlags flags, bool safemode);
    }
}