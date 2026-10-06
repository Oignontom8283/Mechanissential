
namespace MechanicaSaveFix.DotNet
{
    /// <summary>
    /// Internal utility methods for .NET Framework 1.0 compatibility.
    /// </summary>
    public class Utilities
    {
        public static string[] RemoveEmptyEntries(string[] source)
        {
            int count = 0;
            for (int i = 0; i < source.Length; i++)
            {
                if (source[i].Length > 0) count++;
            }

            string[] result = new string[count];
            int idx = 0;
            for (int i = 0; i < source.Length; i++)
            {
                if (source[i].Length > 0)
                {
                    result[idx] = source[i];
                    idx++;
                }
            }
            return result;
        }
    }
}