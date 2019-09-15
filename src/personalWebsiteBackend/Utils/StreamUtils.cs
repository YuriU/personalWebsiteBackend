using System;
using System.IO;

namespace personalWebsiteBackend.Utils
{
    public static class StreamUtils
    {
        public static string ToBase64String(this Stream stream)
        {
            byte[] bytes;
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                bytes = memoryStream.ToArray();
            }

            string base64 = Convert.ToBase64String(bytes);
            return base64;
        }
    }
}