using Cephei.Tools;
using System;
using System.Collections.Generic;

namespace Cephei.Objects
{
  /// <summary>
  /// The ObjectExtensions class contains extension methods related to Cephei's Object library.
  /// </summary>
  public static class ObjectExtensions
  {
    /// <summary>
    /// Copies all objects in a collection to a list.
    /// </summary>
    /// <typeparam name="T">The ICopiable object type.</typeparam>
    /// <param name="en">Collection of copiable objects.</param>
    /// <returns>The list containing the copied items.</returns>
    public static List<T> CloneAll<T>(this IEnumerable<T> en) where T : ICloneable<T>
    {
      List<T> list = new List<T>();
      foreach (ICloneable<T> copiable in en) list.Add(copiable.Clone());
      return list;
    }
    /// <summary>
    /// Copies all objects in a collection to a list.
    /// </summary>
    /// <typeparam name="T">The parameter type used by the ICopiable object.</typeparam>
    /// <typeparam name="U">The ICopiable object type.</typeparam>
    /// <param name="en">Collection of copiable objects.</param>
    /// <param name="parm">Parameter to use for copying the objects.</param>
    /// <returns>The list containing the copied items.</returns>
    public static List<U> CloneAll<T, U>(this IEnumerable<U> en, T parm) where U : ICloneable<T, U>
    {
      List<U> list = new List<U>();
      foreach (ICloneable<T, U> copiable in en) list.Add(copiable.Clone(parm));
      return list;
    }
    /// <summary>
    /// Copies all objects in a collection to a list.
    /// </summary>
    /// <typeparam name="T1">The first parameter type used by the ICopiable object.</typeparam>
    /// <typeparam name="T2">The second parameter type used by the ICopiable object.</typeparam>
    /// <typeparam name="U">The ICopiable object type.</typeparam>
    /// <param name="en">Collection of copiable objects.</param>
    /// <param name="par1">First parameter to use for copying the objects.</param>
    /// <param name="par2">Second parameter to use for copying the objects.</param>
    /// <returns>The list containing the copied items.</returns>
    public static List<U> CloneAll<T1, T2, U>(this IEnumerable<U> en, T1 par1, T2 par2) where U : ICloneable<T1, T2, U>
    {
      List<U> list = new List<U>();
      foreach (ICloneable<T1, T2, U> copiable in en) list.Add(copiable.Clone(par1, par2));
      return list;
    }
    /// <summary>
    /// Copies all objects in a collection to a list.
    /// </summary>
    /// <typeparam name="T1">The first parameter type used by the ICopiable object.</typeparam>
    /// <typeparam name="T2">The second parameter type used by the ICopiable object.</typeparam>
    /// <typeparam name="T3">The third parameter type used by the ICopiable object.</typeparam>
    /// <typeparam name="U">The ICopiable object type.</typeparam>
    /// <param name="en">Collection of copiable objects.</param>
    /// <param name="par1">First parameter to use for copying the objects.</param>
    /// <param name="par2">Second parameter to use for copying the objects.</param>
    /// <param name="par3">Third parameter to use for copying the objects.</param>
    /// <returns>The list containing the copied items.</returns>
    public static List<U> CloneAll<T1, T2, T3, U>(this IEnumerable<U> en, T1 par1, T2 par2, T3 par3) where U : ICloneable<T1, T2, T3, U>
    {
      List<U> list = new List<U>();
      foreach (ICloneable<T1, T2, T3, U> copiable in en) list.Add(copiable.Clone(par1, par2, par3));
      return list;
    }

    /// <summary>
    /// Tries to get an object from a [T IIdentifiable] dictionary. It works the same as dictionary[T] but it will handle the KeyNotFoundException differently,
    /// meaning that an ObjectDoesNotExistException will be thrown instead, referencing another object (if available).
    /// </summary>
    /// <typeparam name="T">The IIdentifiable type.</typeparam>
    /// <typeparam name="U">The IIdentifiable object type.</typeparam>
    /// <param name="dict">The [T IIdentifiable] dictionary.</param>
    /// <param name="key">The T Key to look for.</param>
    /// <param name="objref">Reference to another involved object.</param>
    /// <returns>The IIdentifiable object.</returns>
    /// <exception cref="ObjectDoesNotExistException"></exception>
    public static U GetObject<T, U>(this IReadOnlyDictionary<T, U> dict, T key, object? objref = null)
    {
      if (!dict.ContainsKey(key)) throw new ObjectDoesNotExistException(typeof(U), key, objref);
      else return dict[key];
    }

    /// <summary>
    /// Gets the index of an identifiable object in a collection.
    /// </summary>
    /// <typeparam name="T">Type used for the identifiable's ID.</typeparam>
    /// <typeparam name="U">The identifiable's object type.</typeparam>
    /// <param name="en">Collection to look into.</param>
    /// <param name="id">ID to look after.</param>
    /// <returns>The object's ID (-1 if not found).</returns>
    public static int IndexOf<T, U>(this IEnumerable<U> en, T id) where U : IReadOnlyIdentifiable<T>
    {
      int c = 0;
      foreach (U obj in en)
      {
        if (obj.ID.SafeEquals(id)) return c;
        c++;
      }
      return -1;
    }

    /// <summary>
    /// Removes an IIdentifiable from a [T IIdentifiable] dictionary with the key based off of its ID.
    /// </summary>
    /// <typeparam name="T">The IIdentifiable type.</typeparam>
    /// <typeparam name="U">The IIdentifiable object.</typeparam>
    /// <param name="dict">The [T IIdentifiable] dictionary.</param>
    /// <param name="obj">Object to be removed.</param>
    /// <returns>True if the object was found and removed, false otherwise.</returns>
    public static bool Remove<T, U>(this IDictionary<T, U> dict, U obj) where U : IReadOnlyIdentifiable<T> => dict.Remove(obj.ID);

    /// <summary>
    /// Checks if one of the IUpdateable objects in a collection is pending an update.
    /// </summary>
    /// <typeparam name="T">The IUpdateable object.</typeparam>
    /// <param name="en">The IUpdateable collection.</param>
    /// <returns>True if one object is pending an update, false otherwise.</returns>
    public static bool IsPendingUpdate<T>(this IEnumerable<T> en) where T : IUpdateable
    {
      foreach (IUpdateable del in en)
      {
        if (!del.Updated) return true;
      }
      return false;
    }

    /// <summary>
    /// Checks if the Deletable object is not null and has been deleted. If it is null or has not been deleted then it will return false.
    /// </summary>
    /// <param name="delet">The deletable object.</param>
    /// <returns>True if the object is not null and has the Deleted flag, false otherwise.</returns>
    public static bool IsDeletedAndNotNull(this IReadOnlyDeletable? delet) => !(delet is null) && delet.Deleted;

    /// <summary>
    /// Tries to delete an IDeletable object. Note that exceptions may still be thrown if they occur during the object's Delete execution.
    /// </summary>
    /// <param name="obj">The object to delete.</param>
    /// <returns>True if the object was deletable.</returns>
    public static bool TryDelete(this IReadOnlyDeletable obj)
    {
      if (obj.Deletable)
      {
        obj.Delete();
        return true;
      }
      else return false;
    }

    /// <summary>
    /// Checks if a read only dictionary contains an IIdentifiable based off of its ID.
    /// </summary>
    /// <typeparam name="T">The IIdentifiable type.</typeparam>
    /// <typeparam name="U">The IIdentifiable object type.</typeparam>
    /// <param name="dict">The IIdentifiable dictionary.</param>
    /// <param name="obj">Object to look for.</param>
    /// <returns>True if the dictionary contains an object with the same ID.</returns>
    public static bool Contains<T, U>(this IReadOnlyDictionary<T, U> dict, U obj) where U : IReadOnlyIdentifiable<T> => dict.ContainsKey(obj.ID);
    /// <summary>
    /// Checks if an identifiable object is present in the specified collection.
    /// </summary>
    /// <typeparam name="T">Type used for the identifiable's ID.</typeparam>
    /// <typeparam name="U">The identifiable's type.</typeparam>
    /// <param name="en">Collection to look into.</param>
    /// <param name="id">The object's ID.</param>
    /// <returns>True if the object is present in the collection.</returns>
    public static bool Contains<T, U>(this IEnumerable<U> en, T id) where U : IReadOnlyIdentifiable<T>
    {
      foreach (U obj in en)
      {
        if (obj.ID.SafeEquals(id)) return true;
      }
      return false;
    }

    /// <summary>
    /// Updates all IUpdateables in a collection.
    /// </summary>
    /// <typeparam name="T">The IUpdateable object.</typeparam>
    /// <param name="en">The collection.</param>
    public static void UpdateAll<T>(this IEnumerable<T> en) where T : IUpdateable
    {
      foreach (T obj in en) obj.Update();
    }

    /// <summary>
    /// Signals all IUpdateables in a collection to update.
    /// </summary>
    /// <typeparam name="T">The IUpdateable object.</typeparam>
    /// <param name="en">The collection.</param>
    public static void SignalUpdateAll<T>(this IEnumerable<T> en) where T : IUpdateable
    {
      foreach (T obj in en) obj.SignalUpdate();
    }

    /// <summary>
    /// Ticks all objects in a collection.
    /// </summary>
    /// <typeparam name="T">The collection type.</typeparam>
    /// <param name="en">The collection.</param>
    public static void TickAll<T>(this IEnumerable<T> en) where T : ITickable
    {
      foreach (T obj in en) obj.Tick();
    }
    /// <summary>
    /// Ticks all objects in a collection for a certain amount.
    /// </summary>
    /// <typeparam name="T">Tickable value type.</typeparam>
    /// <typeparam name="U">Tickable object.</typeparam>
    /// <param name="en">The collection.</param>
    /// <param name="amount">Amount to tick it for.</param>
    public static void TickAll<T, U>(this IEnumerable<U> en, T amount) where U : ITickable<T>
    {
      foreach (U obj in en) obj.Tick(amount);
    }

    /// <summary>
    /// Deletes all IDeletable objects in a collection.
    /// </summary>
    /// <typeparam name="T">The IDeletable object type.</typeparam>
    /// <param name="en">Collection of IDeletable objects;</param>
    /// <exception cref="ObjectNotDeletableException"></exception>
    public static void DeleteAll<T>(this IEnumerable<T> en) where T : IReadOnlyDeletable
    {
      foreach (T obj in en) obj.Delete();
    }
    /// <summary>
    /// Deletes all IDeletable objects in a collection, collecting all ObjectNotDeletable exceptions.
    /// </summary>
    /// <typeparam name="T">The IDeletable object type.</typeparam>
    /// <param name="en">Collection of objects.</param>
    /// <param name="excs">List of exceptions.</param>
    public static void DeleteAll<T>(this IEnumerable<T> en, out List<ObjectNotDeletableException> excs) where T : IReadOnlyDeletable
    {
      excs = new List<ObjectNotDeletableException>();
      foreach (T obj in en)
      {
        try { obj.Delete(); }
        catch (ObjectNotDeletableException e) { excs.Add(e); }
      }
    }

    /// <summary>
    /// Disposes all objects in a collection.
    /// </summary>
    /// <typeparam name="T">The IDisposable object type.</typeparam>
    /// <param name="en">Collection of objects to be disposed.</param>
    public static void DisposeAll<T>(this IEnumerable<T> en) where T : IDisposable
    {
      foreach (T obj in en) obj.Dispose();
    }

    /// <summary>
    /// Sets the deletable flag of all objects in a collection.
    /// </summary>
    /// <typeparam name="T">The IDeletable object type.</typeparam>
    /// <param name="en">The collection of objects.</param>
    /// <param name="val">Value to set their deletable flags to.</param>
    public static void SetDeletable<T>(this IEnumerable<T> en, bool val = false) where T : IDeletable
    {
      foreach (T obj in en) obj.Deletable = val;
    }

    /// <summary>
    /// Sets the modifiable flag of all objects in a collection.
    /// </summary>
    /// <typeparam name="T">The IModifiable object.</typeparam>
    /// <param name="en">The collection of objects.</param>
    /// <param name="val">The value to set their modifiable flags to.</param>
    /// <exception cref="ObjectNotModifiableException"></exception>
    public static void SetModifiable<T>(this IEnumerable<T> en, bool val = false) where T : IModifiable
    {
      foreach (T obj in en) obj.Modifiable = val;
    }

    /// <summary>
    /// Sets the deletable and modifiable flag of all objects in a collection. Does nothing to objects that are already not modifiable.
    /// </summary>
    /// <typeparam name="T">The IModifiable and IDeletable objects.</typeparam>
    /// <param name="en">Collection of objects.</param>
    /// <param name="del">Value to set their deletable flags to.</param>
    /// <param name="mod">Value to set their modifiable flags to.</param>
    public static void SetDeletableAndModifiable<T>(this IEnumerable<T> en, bool del = false, bool mod = false) where T : IDeletable, IModifiable
    {
      foreach (T obj in en)
      {
        if (!obj.Modifiable) continue;
        obj.Deletable = del;
        obj.Modifiable = mod;
      }
    }

    /// <summary>
    /// Adds all IIdentifiables in an Enumerable to a dictionary.
    /// </summary>
    /// <typeparam name="T">The IIdentifiable type.</typeparam>
    /// <typeparam name="U">The IIdentifiable object.</typeparam>
    /// <param name="dict">The [T IIdentifiable] dictionary.</param>
    /// <param name="en">Collection of IIdentifiables to add.</param>
    public static void AddAll<T, U>(this IDictionary<T, U> dict, IEnumerable<U> en) where U : IReadOnlyIdentifiable<T>
    {
      foreach (U ident in en) dict.Add(ident);
    }
    /// <summary>
    /// Adds all IIdentifiables in an Enumerable to a dictionary, adding all ArgumentExceptions to a list.
    /// </summary>
    /// <typeparam name="T">The IIdentifiable type.</typeparam>
    /// <typeparam name="U">The IIdentifiable object.</typeparam>
    /// <param name="dict">The {T IIdentifiable} dictionary.</param>
    /// <param name="en">Collection of IIdentifiables to add.</param>
    /// <param name="excs">List of ArgumentExceptions.</param>
    public static void AddAll<T, U>(this IDictionary<T, U> dict, IEnumerable<U> en, out List<ArgumentException> excs) where U : IReadOnlyIdentifiable<T>
    {
      excs = new List<ArgumentException>();
      foreach (U ident in en)
      {
        try { dict.Add(ident); }
        catch (ArgumentException e) { excs.Add(e); }
      }
    }

    /// <summary>
    /// Adds an IIdentifiable object to a [T IIdentifiable] dictionary, assigning its ID to the key.
    /// </summary>
    /// <typeparam name="T">Any IIdentifiable type.</typeparam>
    /// <typeparam name="U">Any IIdentifiable object.</typeparam>
    /// <param name="dict">The [T IIdentifiable] dictionary.</param>
    /// <param name="obj">The IIdentifiable object.</param>
    public static void Add<T, U>(this IDictionary<T, U> dict, U obj) where U : IReadOnlyIdentifiable<T> => dict.Add(obj.ID, obj);
  }
}
