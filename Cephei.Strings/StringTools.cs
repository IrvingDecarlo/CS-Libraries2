using System.Collections.Generic;
using System.Text;

namespace Cephei.Strings
{
  /// <summary>
  /// The StringTools class contains static methods for strings.
  /// </summary>
  public static class StringTools
  {
    /// <summary>
    /// Returns a string containing a specified amount of backspaces. May also include empty spaces to represent deletion.
    /// </summary>
    /// <param name="amount">Number of spaces to backtrack.</param>
    /// <param name="erase">Erase characters?</param>
    /// <returns>The string representing the backspaces and deletions.</returns>
    public static string Backspace(int amount = 1, bool erase = true)
    {
      string bsp = new string('\b', amount);
      if (erase) bsp += new string(' ', amount) + bsp;
      return bsp;
    }
  }
}
