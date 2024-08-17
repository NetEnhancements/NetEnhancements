using Microsoft.Extensions.Primitives;

namespace NetEnhancements.AspNet.StaticFiles
{
    internal static class PathUtils
    {
        internal static readonly char[] PathSeparators = new[]
            {Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar};

        private static char[] GetInvalidFileNameChars() => Path.GetInvalidFileNameChars()
            .Where(c => c != Path.DirectorySeparatorChar && c != Path.AltDirectorySeparatorChar).ToArray();

        private static readonly char[] _invalidFileNameChars = GetInvalidFileNameChars();

        internal static bool HasInvalidPathChars(string path) => path.IndexOfAny(_invalidFileNameChars) >= 0;

        internal static string EnsureTrailingSlash(string path)
        {
            if (!string.IsNullOrEmpty(path) && path[^1] != Path.DirectorySeparatorChar)
            {
                return path + Path.DirectorySeparatorChar;
            }

            return path;
        }

        internal static bool PathNavigatesAboveRoot(string path)
        {
            var tokenizer = new StringTokenizer(path, PathSeparators);
            int depth = 0;

            foreach (StringSegment segment in tokenizer)
            {
                if (segment.Equals(".") || segment.Equals(""))
                {
                    continue;
                }
                
                if (segment.Equals(".."))
                {
                    depth--;

                    if (depth == -1)
                    {
                        return true;
                    }
                }
                else
                {
                    depth++;
                }
            }

            return false;
        }
    }
}
