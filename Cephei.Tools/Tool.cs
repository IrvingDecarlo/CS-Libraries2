using System;

namespace Cephei.Tools
{
  /// <summary>
  /// The static Tool class contains a set of common use methods.
  /// </summary>
  public static class Tool
  {
    /// <summary>
    /// Returns the biggest value within a collection.
    /// </summary>
    /// <typeparam name="T">Object type to be validated.</typeparam>
    /// <param name="parms">Collection of values to compare.</param>
    /// <returns>The biggest value in the collection.</returns>
    public static T Max<T>(params T[] parms) where T : IComparable<T>
    {
      T val = parms[0];
      T ix;
      for (int i = 1; i < parms.Length; i++)
      {
        ix = parms[i];
        if (ix.CompareTo(val) > 0) val = ix;
      }
      return val;
    }

    /// <summary>
    /// Returns the smallest value within a collection.
    /// </summary>
    /// <typeparam name="T">Object type to be validated.</typeparam>
    /// <param name="parms">Collection of values to compare.</param>
    /// <returns>The smallest value in the collection.</returns>
    public static T Min<T>(params T[] parms) where T : IComparable<T> 
    {
      T val = parms[0];
      T ix;
      for (int i = 1; i < parms.Length; i++)
      {
        ix = parms[i];
        if (ix.CompareTo(val) < 0) val = ix;
      }
      return val;
    }

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
