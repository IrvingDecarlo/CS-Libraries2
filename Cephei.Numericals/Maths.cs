namespace Cephei.Numericals
{
  /// <summary>
  /// The Maths class contains extension methods related to mathematics.
  /// </summary>
  public static class Maths
  {
    #region public

    /// <summary>
    /// Gets the largest value out of a collection of integers.
    /// </summary>
    /// <param name="values">Collection of values.</param>
    /// <returns>The largest value.</returns>
    public static int Max(params int[] values)
    {
      int v = 0;
      int val;
      for (int i = 0; i < values.Length; i++)
      {
        val = values[i];
        if (val > v) v = val;
      }
      return v;
    }

    /// <summary>
    /// Gets the smallest value out of a collection of integers.
    /// </summary>
    /// <param name="values">Collection of values.</param>
    /// <returns>The smallest value.</returns>
    public static int Min(params int[] values)
    {
      int v = 0;
      int val;
      for (int i = 0; i < values.Length; i++)
      {
        val = values[i];
        if (val < v) v = val;
      }
      return v;
    }

    #endregion

    #region private

    #endregion
  }
}
