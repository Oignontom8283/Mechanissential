
using System;
using System.Globalization;
using System.Text;
using sPath = System.IO.Path;

namespace MechanicaSaveFix.DotNet
{
    public static class Path
    {
        
        /// <summary>
        /// Reproduces Path.GetRelativePath(string, string) from .NET Core,
        /// compatible with .NET Framework 1.0.
        /// </summary>
        public static string GetRelativePath(string relativeTo, string path)
        {
            if (relativeTo == null)
                throw new ArgumentNullException("relativeTo");
            if (path == null)
                throw new ArgumentNullException("path");
            if (relativeTo.Length == 0)
                throw new ArgumentException("The value cannot be empty.", "relativeTo");
            if (path.Length == 0)
                throw new ArgumentException("The value cannot be empty.", "path");

            string fullRelativeTo = sPath.GetFullPath(relativeTo);
            string fullPath = sPath.GetFullPath(path);

            string rootRelativeTo = sPath.GetPathRoot(fullRelativeTo);
            string rootPath = sPath.GetPathRoot(fullPath);

            if (string.Compare(rootRelativeTo, rootPath, true, CultureInfo.InvariantCulture) != 0)
            {
                return fullPath;
            }

            char[] separators = new char[] { sPath.DirectorySeparatorChar, sPath.AltDirectorySeparatorChar };

            string[] splitRelativeTo = fullRelativeTo.Split(separators);
            string[] splitPath = fullPath.Split(separators);

            splitRelativeTo = Utilities.RemoveEmptyEntries(splitRelativeTo);
            splitPath = Utilities.RemoveEmptyEntries(splitPath);

            int commonLength = 0;
            int minLength = Math.Min(splitRelativeTo.Length, splitPath.Length);

            while (commonLength < minLength &&
                string.Compare(splitRelativeTo[commonLength], splitPath[commonLength], true, CultureInfo.InvariantCulture) == 0)
            {
                commonLength++;
            }

            StringBuilder sb = new StringBuilder();

            for (int i = commonLength; i < splitRelativeTo.Length; i++)
            {
                if (sb.Length > 0) sb.Append(sPath.DirectorySeparatorChar);
                sb.Append("..");
            }

            for (int i = commonLength; i < splitPath.Length; i++)
            {
                if (sb.Length > 0) sb.Append(sPath.DirectorySeparatorChar);
                sb.Append(splitPath[i]);
            }

            if (sb.Length == 0)
                return ".";

            return sb.ToString();
        }
    }
}