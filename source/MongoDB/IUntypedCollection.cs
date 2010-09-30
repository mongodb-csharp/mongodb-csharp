using System.Collections;

namespace MongoDB
{
    internal interface IUntypedCollection
    {
        long Count();

        long Count(object selector);

        IUntypedCursor FindAll();

        IUntypedCursor Find(object spec);

        object FindAndModify(object update, object selector, object sort, object fields, bool returnNew, bool remove, bool upsert);

        void Insert(object document, bool safemode);

        void InsertMany(IEnumerable documents, bool safemode);

        MapReduce MapReduce();

        void Remove(object selector, RemoveFlags flags, bool safemode);

        void Save(object document, bool safemode);

        void Update(object document, object selector, UpdateFlags flags, bool safemode);
    }
}