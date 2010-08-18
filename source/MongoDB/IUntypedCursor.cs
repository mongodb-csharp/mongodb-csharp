using System;
using System.Collections;

namespace MongoDB
{
    internal interface IUntypedCursor : IDisposable
    {
        void Spec(object spec);

        void Limit(int limit);

        void Skip(int skip);

        void Fields(object fields);

        void Options(QueryOptions options);

        void Sort(object sort);

        void Hint(object index);

        void KeepCursor(bool value);

        void Snapshot();

        Document Explain();

        bool IsModifiable { get; }

        IEnumerable Documents { get; }
    }
}