using System;

namespace Cephei.Objects
{
  /// <summary>
  /// ICopiable denotes that an object is capable of being copied.
  /// </summary>
  /// <typeparam name="T">The type of object created when Copy is called.</typeparam>
  public interface ICloneable<T> : ICloneable
  {
    /// <summary>
    /// Copies the object.
    /// </summary>
    /// <returns>The copy of the original object.</returns>
    new T Clone();
  }
  /// <summary>
  /// ICopiable denotes an object that may be copied using a parameter.
  /// </summary>
  /// <typeparam name="T">Parameter type to be used for copying.</typeparam>
  /// <typeparam name="U">Copied object type.</typeparam>
  public interface ICloneable<T, U>
  {
    /// <summary>
    /// Copies the object using a parameter.
    /// </summary>
    /// <param name="parm">Parameter to use for copying the object.</param>
    /// <returns>The copied object.</returns>
    U Clone(T parm);
  }
  /// <summary>
  /// ICopiable denotes an object that may be copied using two parameters.
  /// </summary>
  /// <typeparam name="T1">First parameter type to be used for copying.</typeparam>
  /// <typeparam name="T2">Second parameter type to be used for copying.</typeparam>
  /// <typeparam name="U">Copied object type.</typeparam>
  public interface ICloneable<T1, T2, U>
  {
    /// <summary>
    /// Copies the object using two parameters.
    /// </summary>
    /// <param name="par1">First parameter to use.</param>
    /// <param name="par2">Second parameter to use.</param>
    /// <returns>The copied object.</returns>
    U Clone(T1 par1, T2 par2);
  }
  /// <summary>
  /// ICopiable denotes an object that may be copied using three parameters.
  /// </summary>
  /// <typeparam name="T1">First parameter type to be used for copying.</typeparam>
  /// <typeparam name="T2">Second parameter type to be used for copying.</typeparam>
  /// <typeparam name="T3">Third parameter type to be used for copying.</typeparam>
  /// <typeparam name="U">Copied object type.</typeparam>
  public interface ICloneable<T1, T2, T3, U>
  {
    /// <summary>
    /// Copies the object using three parameters.
    /// </summary>
    /// <param name="par1">First parameter to use.</param>
    /// <param name="par2">Second parameter to use.</param>
    /// <param name="par3">Third parameter to use.</param>
    /// <returns>The copied object.</returns>
    U Clone(T1 par1, T2 par2, T3 par3);
  }
}
