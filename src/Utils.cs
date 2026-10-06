
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

}