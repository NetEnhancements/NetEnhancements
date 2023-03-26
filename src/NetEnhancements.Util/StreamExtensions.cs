using System.IO;

namespace NetEnhancements.Util
{
    public static class StreamExtensions
    {
        /// <summary>
        /// Read a strem entirely, optinoally not disposing it.
        /// </summary>
        public static byte[] ReadFully(this Stream stream, bool dispose = true)
        {
            using MemoryStream ms = new();

            stream.CopyTo(ms);
            
            if (dispose)
            { 
                // We're done with the source stream, and so should the caller.
                stream.Dispose();
            }

            return ms.ToArray();
        }
    }
}
