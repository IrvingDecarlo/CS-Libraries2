using Cephei.Collections;
using Cephei.Objects;
using Cephei.Tools;
using System;
using System.Collections.Generic;

namespace Cephei.Commands.Consoles
{
  /// <summary>
  /// The InputCollection class contains the user's input history.
  /// </summary>
  public sealed class InputCollection : ProtectedList<string>, IList<string>, IPositionable<int>
  {
    internal InputCollection() : base() 
    { }

    #region overrides

    /// <summary>
    /// Gets the input in a certain position. Cannot be set.
    /// </summary>
    /// <param name="i">Position to get the input from.</param>
    /// <returns>The input in the defined position.</returns>
    /// <exception cref="NotSupportedException"></exception>
    public new string this[int i]
    {
      set => throw new NotSupportedException();
      get => base[i];
    }

    /// <summary>
    /// Inserts a command in position. Is always not supported.
    /// </summary>
    /// <param name="index">Position to insert the command in.</param>
    /// <param name="value">The input to be inserted</param>
    /// <exception cref="NotSupportedException"></exception>
    public void Insert(int index, string value) => throw new NotSupportedException();

    /// <summary>
    /// Removes an input from a certain position. Not supported.
    /// </summary>
    /// <param name="index">Position to remove the input from.</param>
    /// <exception cref="NotSupportedException"></exception>
    public void RemoveAt(int index) => throw new NotSupportedException();

    /// <summary>
    /// Adds an input to the history.
    /// </summary>
    /// <param name="value">Input to add to the command history.</param>
    public void Add(string value)
    {
      Collection.Add(value);
      pos = Count - 1;
    }

    /// <summary>
    /// Clears the input history.
    /// </summary>
    public void Clear()
    {
      Collection.Clear();
      pos = 0;
    }

    /// <summary>
    /// Removes a certain input from the history. Not supported.
    /// </summary>
    /// <param name="value">Input to remove from the command history.</param>
    /// <returns>True if the input was found and removed.</returns>
    /// <exception cref="NotSupportedException"></exception>
    public bool Remove(string value) => throw new NotSupportedException();

    /// <summary>
    /// Is the input history ReadOnly? Returns false by default.
    /// </summary>
    public bool IsReadOnly => false;

    /// <summary>
    /// Gets or sets the position within the input history.
    /// </summary>
    public int Position
    {
      set => pos = value.Clamp(Count - 1, 0);
      get => pos;
    }

    /// <summary>
    /// Moves the position to the next command that was inputted.
    /// </summary>
    /// <param name="amount">Amount to move forward.</param>
    /// <returns>The new position.</returns>
    public int MoveNext(int amount = 1)
    {
      pos = Math.Min(pos + amount, Count - 1);
      return pos;
    }

    /// <summary>
    /// Moves the position to the previous command that was inputted.
    /// </summary>
    /// <param name="amount">Amount to move behind.</param>
    /// <returns>The new position.</returns>
    public int MovePrevious(int amount = 1)
    {
      pos = Math.Max(pos - amount, 0);
      return pos;
    }

    #endregion

    #region private

    private int pos;

    #endregion
  }
}
