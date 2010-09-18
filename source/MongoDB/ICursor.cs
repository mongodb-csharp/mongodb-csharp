using System;
using System.Collections.Generic;
using System.Collections;

namespace MongoDB
{
    internal interface ICursor : IDisposable
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
       

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICursor<T> : IDisposable
    {
        /// <summary>
        /// Gets the id.
        /// </summary>
        /// <value>The id.</value>
        long Id { get; }

        /// <summary>
        /// Specifies the selector.
        /// </summary>
        /// <param name="spec">The spec.</param>
        /// <returns></returns>
        ICursor<T> Spec(Document spec);

        /// <summary>
        /// Specifies the selector.
        /// </summary>
        /// <param name="spec">The spec.</param>
        /// <returns></returns>
        ICursor<T> SpecByExample<TExample>(TExample spec);

        /// <summary>
        /// Limits the specified limit.
        /// </summary>
        /// <param name="limit">The limit.</param>
        /// <returns></returns>
        ICursor<T> Limit(int limit);

        /// <summary>
        /// Skips the specified skip.
        /// </summary>
        /// <param name="skip">The skip.</param>
        /// <returns></returns>
        ICursor<T> Skip(int skip);

        /// <summary>
        /// Specifies the projection.
        /// </summary>
        /// <param name="fields">The fields.</param>
        /// <returns></returns>
        ICursor<T> Fields(Document fields);

        /// <summary>
        /// Specifies the projection.
        /// </summary>
        /// <param name="fields">The fields.</param>
        /// <returns></returns>
        ICursor<T> FieldsByExample<TExample>(TExample fields);

        /// <summary>
        /// Optionses the specified options.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        ICursor<T> Options(QueryOptions options);

        /// <summary>
        /// Sorts the specified fields.
        /// </summary>
        /// <param name="fields">The fields.</param>
        /// <returns></returns>
        ICursor<T> Sort(Document fields);

        /// <summary>
        /// Sorts the specified fields.
        /// </summary>
        /// <param name="fields">The fields.</param>
        /// <returns></returns>
        ICursor<T> SortByExample<TExample>(TExample fields);

        /// <summary>
        /// Hints the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        ICursor<T> Hint(Document index);

        /// <summary>
        /// Hints the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        ICursor<T> HintByExample<TExample>(TExample index);

        /// <summary>
        /// Keeps the cursor open.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns></returns>
        /// <remarks>
        /// By default cursors are closed automaticly after documents 
        /// are Enumerated. 
        /// </remarks>
        ICursor<T> KeepCursor(bool value);

        /// <summary>
        /// Snapshots this instance.
        /// </summary>
        /// <returns></returns>
        ICursor<T> Snapshot();

        /// <summary>
        /// Explains this instance.
        /// </summary>
        /// <returns></returns>
        Document Explain();

        /// <summary>
        /// Gets a value indicating whether this instance is modifiable.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is modifiable; otherwise, <c>false</c>.
        /// </value>
        bool IsModifiable { get; }

        /// <summary>
        /// Gets the documents.
        /// </summary>
        /// <value>The documents.</value>
        IEnumerable<T> Documents { get; }
    }
}
