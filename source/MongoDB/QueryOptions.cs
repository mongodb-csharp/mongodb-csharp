using System;

namespace MongoDB
{
    /// <summary>
    ///   Query options
    /// </summary>
    /// <remarks>
    ///   Oplog replay: 8 (internal replication use only - drivers should not implement)
    /// </remarks>
    [Flags]
    public enum QueryOptions
    {
        /// <summary>
        ///   None
        /// </summary>
        None = 0,
        /// <summary>
        ///   Tailable means cursor is not closed when the last data is retrieved. Rather, 
        ///   the cursor marks the final object's position. You can resume using the cursor 
        ///   later, from where it was located, if more data were received. Like any "latent cursor", 
        ///   the cursor may become invalid at some point (CursorNotFound) – for example if the final 
        ///   object it references were deleted.
        /// </summary>
        TailableCursor = 2,
        /// <summary>
        ///   Allow query of replica slave. Normally these return an error except for namespace "local".
        /// </summary>
        SlaveOk = 4,
        /// <summary>
        ///   The server normally times out idle cursors after an inactivity period (10 minutes)
        ///   to prevent excess memory use. Set this option to prevent that.
        /// </summary>
        NoCursorTimeout = 16,
        /// <summary>
        ///   Use with TailableCursor. If we are at the end of the data, block for a while rather 
        ///   than returning no data. After a timeout period, we do return as normal.
        /// </summary>
        AwaitData = 32,
        /// <summary>
        ///   Stream the data down full blast in multiple "more" packages, on the assumption that the 
        ///   client will fully read all data queried. Faster when you are pulling a lot of data and 
        ///   know you want to pull it all down. Note: the client is not allowed to not read all the 
        ///   data unless it closes the connection.
        /// </summary>
        Exhaust = 64
    }
}