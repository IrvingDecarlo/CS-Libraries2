using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Cephei.Collections
{
  /// <summary>
  /// A class containing extensions for collections.
  /// </summary>
  public static class CollectionExtensions
  {
    /// <summary>
    /// Returns a Protected version of the dictionary.
    /// </summary>
    /// <typeparam name="T">The dictionary's key type.</typeparam>
    /// <typeparam name="U">The dictionary's value type.</typeparam>
    /// <typeparam name="V">The dictionary type.</typeparam>
    /// <param name="dict">The collection to base off of.</param>
    /// <returns>A protected version of the collection.</returns>
    public static ProtectedDictionary<T, U, V> ToProtected<T, U, V>(this V dict) where V : IDictionary<T, U> => new ProtectedDictionary<T, U, V>(dict);
    /// <summary>
    /// Returns a Protected version of the list.
    /// </summary>
    /// <typeparam name="T">The list's object type.</typeparam>
    /// <typeparam name="U">The list type.</typeparam>
    /// <param name="list">The collection to base off of.</param>
    /// <returns>A protected version of the collection.</returns>
    public static ProtectedList<T, U> ToProtected<T, U>(this U list) where U : IList<T> => new ProtectedList<T, U>(list);

    /// <summary>
    /// Returns a Read-only version of a dictionary.
    /// </summary>
    /// <typeparam name="T">The dictionary's key type.</typeparam>
    /// <typeparam name="U">The dictionary's value type.</typeparam>
    /// <param name="dict">The dictionary to be returned as read-only.</param>
    /// <returns>The input dictionary with a read-only wrapper.</returns>
    public static ReadOnlyDictionary<T, U> ToReadOnly<T, U>(this IDictionary<T, U> dict) => new ReadOnlyDictionary<T, U>(dict);

    /// <summary>
    /// Converts an enumerable composed of KeyValuePairs into a Dictionary.
    /// </summary>
    /// <typeparam name="T">The dictionary's key type.</typeparam>
    /// <typeparam name="U">The dictionary's value type.</typeparam>
    /// <param name="collection">Collection of KeyValuePairs.</param>
    /// <returns>A dictionary with the incoming KeyValuePairs.</returns>
    public static Dictionary<T, U> ToDictionary<T, U>(this IEnumerable<KeyValuePair<T, U>> collection)
    {
      Dictionary<T, U> dict = new Dictionary<T, U>();
      foreach (KeyValuePair<T, U> kvp in collection) dict.Add(kvp.Key, kvp.Value);
      return dict;
    }

    /// <summary>
    /// Gets all the value collections from a collection of dictionaries.
    /// </summary>
    /// <typeparam name="T">The dictionaries' key type.</typeparam>
    /// <typeparam name="U">The dictionaries' value type.</typeparam>
    /// <param name="dicts">Collection of dictionaries.</param>
    /// <returns>A list containing the dictionaries' values.</returns>
    public static List<ICollection<U>> GetDictionaryValues<T, U>(this IEnumerable<IDictionary<T, U>> dicts)
    {
      List<ICollection<U>> cols = new List<ICollection<U>>();
      foreach (IDictionary<T, U> dict in dicts) cols.Add(dict.Values);
      return cols;
    }
    /// <summary>
    /// Gets all the value collections from a collection of dictionaries.
    /// </summary>
    /// <param name="dicts">Collection of dictionaries.</param>
    /// <returns>A list containing the dictionaries' values.</returns>
    public static List<ICollection> GetDictionaryValues(this IEnumerable<IDictionary> dicts)
    {
      List<ICollection> cols = new List<ICollection>();
      foreach (IDictionary dict in dicts) cols.Add(dict.Values);
      return cols;
    }

    /// <summary>
    /// Gets all the key collections from a collection of dictionaries.
    /// </summary>
    /// <typeparam name="T">The dictionaries' key type.</typeparam>
    /// <typeparam name="U">The dictionaries' value type.</typeparam>
    /// <param name="dicts">Collection of dictionaries.</param>
    /// <returns>A list containing the dictionaries' keys.</returns>
    public static List<ICollection<T>> GetDictionaryKeys<T, U>(this IEnumerable<IDictionary<T, U>> dicts)
    {
      List<ICollection<T>> cols = new List<ICollection<T>>();
      foreach (IDictionary<T, U> dict in dicts) cols.Add(dict.Keys);
      return cols;
    }
    /// <summary>
    /// Gets all the key collections from a collection of dictionaries.
    /// </summary>
    /// <param name="dicts">Collection of dictionaries.</param>
    /// <returns>A list containing the dictionaries' keys.</returns>
    public static List<ICollection> GetDictionaryKeys(this IEnumerable<IDictionary> dicts)
    {
      List<ICollection> cols = new List<ICollection>();
      foreach (IDictionary dict in dicts) cols.Add(dict.Keys);
      return cols;
    }

    /// <summary>
    /// Gets all enumerators from a collection of enumerables.
    /// </summary>
    /// <typeparam name="T">The collection object type.</typeparam>
    /// <param name="cols">Collection of enumerables.</param>
    /// <returns>The enumerators drawn from the collection of enumerables.</returns>
    public static List<IEnumerator<T>> GetEnumerators<T>(this IEnumerable<IEnumerable<T>> cols)
    {
      List<IEnumerator<T>> enums = new List<IEnumerator<T>>();
      foreach (IEnumerable<T> @enum in cols) enums.Add(@enum.GetEnumerator());
      return enums;
    }
    /// <summary>
    /// Gets all non-generic enumerators from a collection of enumerables.
    /// </summary>
    /// <param name="cols">Collection of enumerables.</param>
    /// <returns>The non-generic enumeratos drawn from the collection of enumerables.</returns>
    public static List<IEnumerator> GetEnumerators(this IEnumerable<IEnumerable> cols)
    {
      List<IEnumerator> enums = new List<IEnumerator>();
      foreach (IEnumerable @enum in cols) enums.Add(@enum.GetEnumerator());
      return enums;
    }

    /// <summary>
    /// Gets or adds a value from a dictionary. If the key doesn't exist, then it is added using a delegate function that returns the new object.
    /// </summary>
    /// <typeparam name="T">The key type.</typeparam>
    /// <typeparam name="U">The value type.</typeparam>
    /// <param name="dict">The dictionary.</param>
    /// <param name="key">The key to use.</param>
    /// <param name="newobj">Function to perform to create the new object.</param>
    /// <returns>The object (if it already exists in the dictionary) or the new object.</returns>
    public static U AddOrGet<T, U>(this IDictionary<T, U> dict, T key, Func<U> newobj)
    {
      if (dict.TryGetValue(key, out U value)) return value;
      value = newobj();
      dict.Add(key, value);
      return value;
    }
    /// <summary>
    /// Gets or adds a value from a dictionary. If the key doesn't exist, then it is added using a delegate function that returns the new object.
    /// </summary>
    /// <param name="dict">The dictionary.</param>
    /// <param name="key">The key to use.</param>
    /// <param name="newobj">Function to perform to create the new object.</param>
    /// <returns>The object (if it already exists in the dictionary) or the new object.</returns>
    public static object AddOrGet(this IDictionary dict, object key, Func<object> newobj)
    {
      if (dict.Contains(key)) return dict[key];
      object value = newobj();
      dict.Add(key, value);
      return value;
    }

    /// <summary>
    /// Combines many objects' strings into a single string, each between quotes ("") and with commas separating them.
    /// </summary>
    /// <param name="en">Collection of objects to combine.</param>
    /// <returns>A string combining all objects.</returns>
    public static string Join(this IEnumerable en)
      => en.Join("\"", "\",\"", "\"");
    /// <summary>
    /// Combines many objects' strings to a single string using additional strings during, prior and post.
    /// </summary>
    /// <param name="en">Collection of objects to combine.</param>
    /// <param name="pre">String to be added prior to the collection.</param>
    /// <param name="mid">String to be added between two objects.</param>
    /// <param name="post">String to be added after the collection.</param>
    /// <returns>A string combining all objects</returns>
    public static string Join(this IEnumerable en, string pre, string mid, string post)
    {
      StringBuilder sb = new StringBuilder(pre);
      bool first = true;
      foreach (object obj in en)
      {
        if (first) first = false;
        else sb.Append(mid);
        sb.Append(obj.ToString());
      }
      sb.Append(post);
      return sb.ToString();
    }

    /// <summary>
    /// Gets the lowest available key int in a dictionary, starting from index.
    /// </summary>
    /// <typeparam name="T">The dictionary's value type.</typeparam>
    /// <param name="dict">The dictionary.</param>
    /// <param name="index">Starting index.</param>
    /// <param name="inc">By how much the index will be incremented each time it fails to find an empty key.</param>
    /// <returns>The first available int key.</returns>
    public static int GetFirstKey<T>(this IReadOnlyDictionary<int, T> dict, int index = 0, int inc = 1)
    {
      while (true)
      {
        if (dict.ContainsKey(index)) index += inc;
        else break;
      }
      return index;
    }
    /// <summary>
    /// Gets the lowest available string key in a dictionary. The formula is str + index.ToString, as index will incease by inc whenever a key is found.
    /// </summary>
    /// <typeparam name="T">The dictionary's value type.</typeparam>
    /// <param name="dict">The dictionary.</param>
    /// <param name="str">Base string value.</param>
    /// <param name="index">Starting index.</param>
    /// <param name="inc">By how much the index will be incremented each time it fails to find an empty key.</param>
    /// <returns>The first available string key.</returns>
    public static string GetFirstKey<T>(this IReadOnlyDictionary<string, T> dict, string str, int index = 0, int inc = 1)
    {
      string key;
      while (true)
      {
        key = str + index.ToString();
        if (dict.ContainsKey(key)) index += inc;
        else break;
      }
      return key;
    }

    /// <summary>
    /// Gets the sum of all Counts in an enumerable of collections.
    /// </summary>
    /// <param name="en">Enumerable of collections.</param>
    /// <returns>Total count.</returns>
    public static int TotalCount(this IEnumerable<ICollection> en)
    {
      int c = 0;
      foreach (ICollection col in en) c += col.Count;
      return c;
    }

    /// <summary>
    /// Returns an index of a specific object within an array.
    /// </summary>
    /// <typeparam name="T">The array type.</typeparam>
    /// <param name="array">The array.</param>
    /// <param name="obj">The object that is being looked for.</param>
    /// <returns>The object's index. Returns -1 if it is not found.</returns>
    public static int IndexOf<T>(this T[] array, T obj)
    {
      T aobj;
      for (int i = 0; i < array.Length; i++)
      {
        aobj = array[i];
        if (!(aobj is null) && aobj.Equals(obj)) return i;
      }
      return -1;
    }

    /// <summary>
    /// Tries to get a value from a dictionary using multiple keys.
    /// </summary>
    /// <typeparam name="T">The key type.</typeparam>
    /// <typeparam name="U">The value type.</typeparam>
    /// <param name="dict">The dictionary.</param>
    /// <param name="value">Value that was collected (default if it does not exist in the dictionary).</param>
    /// <param name="def">Default value to return if the key does not exist.</param>
    /// <param name="keys">Keys to use.</param>
    /// <returns>True if the object was found, false otherwise.</returns>
    public static bool TryGetValue<T, U>(this IReadOnlyDictionary<T, U> dict, out U value, U def, params T[] keys)
    {
      foreach (T key in keys)
      {
        if (dict.TryGetValue(key, out value)) return true;
      }
      value = def;
      return false;
    }
    /// <summary>
    /// Tries to get a value from a dictionary. If it doesn't exist, returns a default value instead.
    /// </summary>
    /// <typeparam name="T">The dictionary's key type.</typeparam>
    /// <typeparam name="U">The dictionary's value type.</typeparam>
    /// <param name="dict">The dictionary.</param>
    /// <param name="key">The key.</param>
    /// <param name="def">Default value to return if the key is not found.</param>
    /// <returns>The object by key or the default object.</returns>
    public static U TryGetValue<T, U>(this IReadOnlyDictionary<T, U> dict, T key, U def)
    {
      if (dict.TryGetValue(key, out U value)) return value;
      else return def;
    }
    /// <summary>
    /// Tries to get a value out of a tabled dictionary.
    /// </summary>
    /// <typeparam name="T">The dictionary's key type.</typeparam>
    /// <typeparam name="U">The dictionary's value type.</typeparam>
    /// <typeparam name="V">The dictionary type.</typeparam>
    /// <param name="dict">Dictionary to get the value from.</param>
    /// <param name="table">Table to get the value from.</param>
    /// <param name="key">Key to get the value.</param>
    /// <param name="value">Value (if found).</param>
    /// <returns>True if the value was found.</returns>
    public static bool TryGetValue<T, U, V>(this V dict, string table, T key, out U value) where V : IReadOnlyDictionary<T, U>, ITabledObject
    {
      dict.Table = table;
      return dict.TryGetValue(key, out value);
    }

    /// <summary>
    /// Adds an object to the collection if it doesn't already exist in it.
    /// </summary>
    /// <typeparam name="T">The collection's object type..</typeparam>
    /// <param name="col">The collection.</param>
    /// <param name="obj">The object to add.</param>
    /// <returns>True if the object was added, false otherwise.</returns>
    public static bool TryAdd<T>(this ICollection<T> col, T obj)
    {
      if (!col.Contains(obj))
      {
        col.Add(obj);
        return true;
      }
      else return false;
    }
    /// <summary>
    /// Adds an object to a list if it doesn't already exist in it.
    /// </summary>
    /// <typeparam name="T">The collection's object type..</typeparam>
    /// <param name="list">The list.</param>
    /// <param name="obj">The object to add.</param>
    /// <param name="movetotop">If the object already exists in the list, should it be moved to the top?</param>
    /// <returns>True if the object was added, false otherwise.</returns>
    public static bool TryAdd<T>(this IList<T> list, T obj, bool movetotop)
    {
      int index = list.IndexOf(obj);
      if (index < 0)
      {
        list.Add(obj);
        return true;
      }
      else
      {
        if (movetotop)
        {
          list.RemoveAt(index);
          list.Add(obj);
        }
        return false;
      }
    }

    /// <summary>
    /// Checks if an array contains an specific object.
    /// </summary>
    /// <typeparam name="T">The array type.</typeparam>
    /// <param name="array">The array.</param>
    /// <param name="obj">The object that is being looked for in the array.</param>
    /// <returns>True if the object is present within the array.</returns>
    public static bool Contains<T>(this T[] array, T obj) => array.IndexOf(obj) >= 0;
    /// <summary>
    /// Checks if a tabled dictionary contains a specified value.
    /// </summary>
    /// <typeparam name="T">Key object type.</typeparam>
    /// <typeparam name="U">Value object type.</typeparam>
    /// <typeparam name="V">Dictionary type.</typeparam>
    /// <param name="dict">Dictionary to use.</param>
    /// <param name="table">Table to get the value from.</param>
    /// <param name="key">Key to get the value from.</param>
    /// <returns>True if the specified key exists in the tabled dictionary.</returns>
    public static bool Contains<T, U, V>(this V dict, string table, T key) where V : IReadOnlyDictionary<T, U>, ITabledObject
    {
      dict.Table = table;
      return dict.ContainsKey(key);
    }

    /// <summary>
    /// Removes an item from a tabled dictionary.
    /// </summary>
    /// <typeparam name="T">Key object type.</typeparam>
    /// <typeparam name="U">Value object type.</typeparam>
    /// <typeparam name="V">Dictionary type.</typeparam>
    /// <param name="dict">Dictionary to use.</param>
    /// <param name="table">Table to remove the value from.</param>
    /// <param name="key">Key to identify the value to be deleted.</param>
    /// <returns>True if the item was found and deleted.</returns>
    public static bool Remove<T, U, V>(this V dict, string table, T key) where V : IDictionary<T, U>, ITabledObject
    {
      dict.Table = table;
      return dict.Remove(key);
    }

    /// <summary>
    /// Resets all enumerators in a collection.
    /// </summary>
    /// <typeparam name="T">The IEnumerator object type.</typeparam>
    /// <param name="enum">Collection of enumerators.</param>
    public static void ResetAll<T>(this IEnumerable<T> @enum) where T : IEnumerator
    {
      foreach (IEnumerator en in @enum) en.Reset();
    }

    /// <summary>
    /// Disposes all disposable objects in a collection.
    /// </summary>
    /// <typeparam name="T">The IDisposable object type.</typeparam>
    /// <param name="disps">Collection of disposables.</param>
    public static void DisposeAll<T>(this IEnumerable<T> disps) where T : IDisposable
    {
      foreach (IDisposable disp in disps) disp.Dispose();
    }

    /// <summary>
    /// Copies the content enumerated by an enumerator to an array.
    /// </summary>
    /// <typeparam name="T">The collection's object type.</typeparam>
    /// <param name="en">The enumerator.</param>
    /// <param name="array">Target array.</param>
    /// <param name="index">Starting index.</param>
    public static void CopyTo<T>(this IEnumerator<T> en, T[] array, int index = 0)
    {
      while (en.MoveNext())
      {
        array[index] = en.Current;
        index++;
      }
    }

    /// <summary>
    /// Adds a collection of objects to a dictionary, using a delegate Func to determine the key that will be used.
    /// </summary>
    /// <typeparam name="T">Key type.</typeparam>
    /// <typeparam name="U">Object type.</typeparam>
    /// <param name="dict">The dictionary to receive the objects.</param>
    /// <param name="keyfunc">Function to determine the key.</param>
    /// <param name="en">Collection of objects to add to the dictionary.</param>
    public static void AddAll<T, U>(this IDictionary<T, U> dict, Func<U, T> keyfunc, IEnumerable<U> en)
    {
      foreach (U obj in en) dict.Add(keyfunc(obj), obj);
    }
    /// <summary>
    /// Adds a collection of objects to a dictionary, using a delegate Func to determine the key that will be used. Outs a list of ArgumentExceptions in case
    /// of duplicate items.
    /// </summary>
    /// <typeparam name="T">Key type.</typeparam>
    /// <typeparam name="U">Object type.</typeparam>
    /// <param name="dict">The dictionary to receive the objects.</param>
    /// <param name="keyfunc">Function to determine the key.</param>
    /// <param name="en">Collection of objects to add to the dictionary.</param>
    /// <param name="excs">List of ArgumentExceptions.</param>
    public static void AddAll<T, U>(this IDictionary<T, U> dict, Func<U, T> keyfunc, IEnumerable<U> en, out List<ArgumentException> excs)
    {
      excs = new List<ArgumentException>();
      foreach (U obj in en)
      {
        try { dict.Add(keyfunc(obj), obj); }
        catch (ArgumentException e) { excs.Add(e); }
      }
    }
    /// <summary>
    /// Adds a collection of objects to another collection.
    /// </summary>
    /// <typeparam name="T">The collection type.</typeparam>
    /// <param name="col">The collection to receive the objects.</param>
    /// <param name="en">The collection of objects to add.</param>
    public static void AddAll<T>(this ICollection<T> col, IEnumerable<T> en)
    {
      foreach (T obj in en) col.Add(obj);
    }

    /// <summary>
    /// Adds an object to any dictionary with Type as key, assigning the object's type as its key.
    /// </summary>
    /// <typeparam name="T">The object type.</typeparam>
    /// <param name="dict">The dictionary.</param>
    /// <param name="obj">Object to add.</param>
    /// <exception cref="ArgumentNullException"></exception>
    public static void Add<T>(this IDictionary<Type, T> dict, T obj) 
      => dict.Add((obj is null ? throw new ArgumentNullException("obj") : obj).GetType(), obj);
    /// <summary>
    /// Adds a key to a tabled dictionary.
    /// </summary>
    /// <typeparam name="T">Key object type.</typeparam>
    /// <typeparam name="U">Value object type.</typeparam>
    /// <typeparam name="V">Dictionary type.</typeparam>
    /// <param name="dict">Dictionary to use.</param>
    /// <param name="table">Target table.</param>
    /// <param name="key">Key to add.</param>
    /// <param name="value">Value to add.</param>
    public static void Add<T, U, V>(this V dict, string table, T key, U value) where V : IDictionary<T, U>, ITabledObject
    {
      dict.Table = table;
      dict.Add(key, value);
    }

    /// <summary>
    /// Adds or sets a key's value in a dictionary.
    /// </summary>
    /// <typeparam name="T">The dictionary's key type.</typeparam>
    /// <typeparam name="U">The dictionary's value type.</typeparam>
    /// <param name="dict">The dictionary.</param>
    /// <param name="key">The key to use.</param>
    /// <param name="value">The value to set the key to.</param>
    public static void AddOrSet<T, U>(this IDictionary<T, U> dict, T key, U value)
    {
      if (dict.ContainsKey(key)) dict[key] = value;
      else dict.Add(key, value);
    }
    /// <summary>
    /// Adds or sets a key's value in a dictionary.
    /// </summary>
    /// <param name="dict">The dictionary.</param>
    /// <param name="key">The key to use.</param>
    /// <param name="value">The value to set the key to.</param>
    public static void AddOrSet(this IDictionary dict, object key, object value)
    {
      if (dict.Contains(key)) dict[key] = value;
      else dict.Add(key, value);
    }
    /// <summary>
    /// Adds or sets a value to a list, adding filler objects if the list's count is insufficient for the index.
    /// </summary>
    /// <typeparam name="T">The list's object type.</typeparam>
    /// <param name="list">List to receive the new object.</param>
    /// <param name="index">Index to set or add the object to.</param>
    /// <param name="value">Value to add to the list.</param>
    /// <param name="filler">Filler objects to insert prior to the index.</param>
    public static void AddOrSet<T>(this IList<T> list, int index, T value, T filler)
    {
      int c = list.Count;
      if (c > index)
      {
        list[index] = value;
        return;
      }
      for (; c < index; c++) list.Add(filler);
      list.Add(value);
    }
    /// <summary>
    /// Adds or sets a value to a list, adding filler objects if the list's count is insufficient for the index.
    /// </summary>
    /// <param name="list">List to receive the new object.</param>
    /// <param name="index">Index to set or add the object to.</param>
    /// <param name="value">Value to add to the list.</param>
    /// <param name="filler">Filler objects to insert prior to the index.</param>
    public static void AddOrSet(this IList list, int index, object value, object filler)
    {
      int c = list.Count;
      if (c > index)
      {
        list[index] = value;
        return;
      }
      for (; c < index; c++) list.Add(filler);
      list.Add(value);
    }

    /// <summary>
    /// Fills the list with a value to all its indexes.
    /// </summary>
    /// <typeparam name="T">The list object type.</typeparam>
    /// <param name="list">The list to be filled.</param>
    /// <param name="value">Value to fill it with.</param>
    /// <param name="i">Starting index.</param>
    public static void Fill<T>(this IList<T> list, T value, int i = 0)
    {
      for (; i < list.Count; i++) list[i] = value;
    }
    /// <summary>
    /// Fills the list with a value to all its indexes.
    /// </summary>
    /// <param name="list">The list to be filled.</param>
    /// <param name="value">Value to fill it with.</param>
    /// <param name="i">Starting index.</param>
    public static void Fill(this IList list, object value, int i = 0)
    {
      for (; i < list.Count; i++) list[i] = value;
    }

    /// <summary>
    /// Is a basic implementation of a CopyTo for enumerables, adding the collection's items to an array.
    /// </summary>
    /// <typeparam name="T">Objec type.</typeparam>
    /// <param name="en">Enumerator to get the items from.</param>
    /// <param name="array">Array to copy the items to.</param>
    /// <param name="i">Starting index.</param>
    public static void DoCopyTo<T>(this IEnumerable<T> en, T[] array, int i = 0)
    {
      foreach (T item in en)
      {
        array[i] = item;
        i++;
      }
    }
  }
}
