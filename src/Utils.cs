
using System;
using System.Collections.Generic;
using UnityEngine;

public class Utils {

    #region Others

    /// <summary>
    /// Counts how many times each distinct item appears in a sequence.
    /// Generic equivalent of the "flat array with duplicates" counting pattern.
    /// </summary>
    /// <typeparam name="T">The item type.</typeparam>
    /// <param name="items">The sequence to count.</param>
    /// <returns>A map from each distinct item to its occurrence count.</returns>
    public static Dictionary<T, int> CountOccurrences<T>(IEnumerable<T> items)
    {
        var result = new Dictionary<T, int>();
        if (items == null) return result;

        foreach (T item in items)
        {
            result[item] = result.TryGetValue(item, out int count) ? count + 1 : 1;
        }

        return result;
    }

    #endregion


    #region String Utilities

    /// <summary>
    /// Compares two version strings (with or without a leading "v").
    /// Returns <c>false</c> if the remote version has any text after the numeric part
    /// (e.g. "-beta", "+build"). Any text after the numeric part of the local version is ignored.
    /// </summary>
    /// <param name="remoteTag">The remote version string (e.g., "v1.2.3").</param>
    /// <param name="localVersion">The local version string (e.g., "1.2.0").</param>
    /// <returns><c>true</c> if the remote version is strictly newer than the local one; otherwise, <c>false</c>.</returns>
    public static bool IsNewerVersion(string remoteTag, string localVersion)
    {
        if (string.IsNullOrWhiteSpace(remoteTag) || string.IsNullOrWhiteSpace(localVersion))
            return false;

        // Remote: no trailing text is tolerated after the version
        string remoteClean = remoteTag.Trim().TrimStart('v', 'V');

        // ignore anything after the numeric part
        string localClean = localVersion.Trim().TrimStart('v', 'V');
        int separatorIndex = localClean.IndexOfAny(new[] { '-', '+', ' ' });
        if (separatorIndex >= 0)
            localClean = localClean.Substring(0, separatorIndex);

        if (!Version.TryParse(remoteClean, out Version remote) ||
            !Version.TryParse(localClean, out Version local))
        {
            return false;
        }

        return NormalizeVersion(remote) > NormalizeVersion(local);
    }

    /// <summary>
    /// Normalizes a <see cref="Version"/> so that all four components are defined.
    /// Unspecified components (Build and Revision, which are -1 when omitted) are set to 0,
    /// so that "1.2" and "1.2.0" are treated as equal when compared.
    /// </summary>
    /// <param name="version">The version to normalize.</param>
    /// <returns>A new <see cref="Version"/> with Build and Revision set to 0 if they were not specified.</returns>
    private static Version NormalizeVersion(Version version) =>
        new Version(version.Major, version.Minor, Math.Max(version.Build, 0), Math.Max(version.Revision, 0));

    #endregion


}