using System;

namespace MongoDB
{
    /// <summary>
    ///   Update flags.
    /// </summary>
    /// <remarks>
    ///   Bits 2-31 are Reserved and must be set to 0.
    /// </remarks>
    [Flags]
    public enum UpdateFlags
    {
        /// <summary>
        ///   Default is None.
        /// </summary>
        Default = 0,
        /// <summary>
        ///   None.
        /// </summary>
        None = 0,
        /// <summary>
        ///   If set, the database will insert the supplied object into the 
        ///   collection if no matching document is found.
        /// </summary>
        Upsert = 1,
        /// <summary>
        ///   If set, the database will update all matching objects in the 
        ///   collection. Otherwise only updates first matching doc.
        /// </summary>
        MultiUpdate = 2
    }
}