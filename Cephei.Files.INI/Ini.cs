using System.Collections;
using System.Collections.Generic;
using System.IO;

using Cephei.Collections;

namespace Cephei.Files.INI
{
  /// <summary>
  /// The Ini class is the main operator for handling Ini files. It imports the file's data, handles it within the class and then exports it back to the file.
  /// Note that names are cAsE sEnSiTiVe.
  /// </summary>
  public class Ini : PersistentFile
    , IDictionary<string, IDictionary<string, string>>
    , IReadOnlyDictionary<string, IDictionary<string, string>>
    , IReadableExceptionFile
  {
    /// <summary>
    /// Creates a new ini object. Does not automatically read its content into it.
    /// </summary>
    /// <param name="path">Path to the ini file.</param>
    public Ini(string path) : this(path, new Dictionary<string, IDictionary<string, string>>())
    { }
    /// <summary>
    /// Creates a new ini object.
    /// </summary>
    /// <param name="path">Path to the target ini file. Its extension should not be added, since it will be automatically added
    /// (or rewritten if it's different to .ini).</param>
    /// <param name="read">If true then the file will be automatically read, with its data imported using standard dictionaries.</param>
    public Ini(string path, bool read) : this(path, new Dictionary<string, IDictionary<string, string>>(), read)
    { }
    /// <summary>
    /// Creates a new ini object with a custom dictionary. Does not read its content into it.
    /// </summary>
    /// <param name="path">Path to the ini file.</param>
    /// <param name="dict">The dictionary to use to organize sections.</param>
    public Ini(string path, IDictionary<string, IDictionary<string, string>> dict) : base(path, "ini")
      => sects = dict;
    /// <summary>
    /// Creates a new ini object while setting a custom IDictionary type to it to organize sections.
    /// </summary>
    /// <param name="path">Path to the ini file.</param>
    /// <param name="dict">The dictionary to use to organize sections.</param>
    /// <param name="read">If true then the file will be automatically read, with its data imported.</param>
    public Ini(string path, IDictionary<string, IDictionary<string, string>> dict, bool read) : this(path, dict)
    {
      if (read) Read();
    }

    //
    // OVERRIDES
    //

    /// <summary>
    /// Reads the ini file's content into this object. Will create section dictionaries automatically if they don't exist.
    /// Nothing will be done if the file does not exist.
    /// </summary>
    public override void Read()
    {
      if (!File.Exists(FilePath)) return;
      new IniReader().Read(this);
    }
    /// <summary>
    /// Reads the ini file's content into this object. Will create section dictionaries automatically if they don't exist.
    /// Nothing will be done if the file does not exist.
    /// </summary>
    /// <param name="excs">List of exceptions that have occurred during file read.</param>
    public virtual void Read(List<FileReadException> excs)
    {
      if (!File.Exists(FilePath)) return;
      new IniExceptionReader().Read(this, excs);
    }

    /// <summary>
    /// Write the ini's content to its respective file.
    /// </summary>
    public override void Write() => new IniWriter().Write(this);

    /// <summary>
    /// Adds a new section to the ini file.
    /// </summary>
    /// <param name="sec">The section's name.</param>
    /// <param name="dict">Dictionary to use as the section's key=value mapper.</param>
    public void Add(string sec, IDictionary<string, string> dict) => sects.Add(sec, dict);
    /// <summary>
    /// Adds a new section to the ini file, using a KeyValuePair.
    /// </summary>
    /// <param name="kvp">The KeyValuePair to add to the Ini.</param>
    public void Add(KeyValuePair<string, IDictionary<string, string>> kvp) => sects.Add(kvp);
    /// <summary>
    /// Adds a new section to the ini file, using a default Dictionary to organize the section's key=values.
    /// </summary>
    /// <param name="sec">The section's name.</param>
    /// <returns>The newly created dictionary for this section.</returns>
    public IDictionary<string, string> Add(string sec)
    {
      IDictionary<string, string> sect = new Dictionary<string, string>();
      sects.Add(sec, sect);
      return sect;
    }

    /// <summary>
    /// Clears all data from this ini file.
    /// </summary>
    public void Clear() => sects.Clear();

    /// <summary>
    /// Copies this file's sections to an array.
    /// </summary>
    /// <param name="kvps">The target array.</param>
    /// <param name="i">Starting index.</param>
    public void CopyTo(KeyValuePair<string, IDictionary<string, string>>[] kvps, int i) => sects.CopyTo(kvps, i);

    /// <summary>
    /// Removes a section from this file.
    /// </summary>
    /// <param name="sec">The section's name.</param>
    /// <returns>True if the section was found and removed.</returns>
    public bool Remove(string sec) => sects.Remove(sec);
    /// <summary>
    /// Removes a section from this file using a KeyValuePair combination.
    /// </summary>
    /// <param name="kvp">The KeyValuePair.</param>
    /// <returns>True if the combination was found and removed.</returns>
    public bool Remove(KeyValuePair<string, IDictionary<string, string>> kvp) => sects.Remove(kvp);

    /// <summary>
    /// Does this ini file contain a certain section?
    /// </summary>
    /// <param name="sec">The section's name.</param>
    /// <returns>True if this section exists within this ini file.</returns>
    public bool ContainsKey(string sec) => sects.ContainsKey(sec);

    /// <summary>
    /// Does this ini contain a certain section, using a certain KeyValuePair combination?
    /// </summary>
    /// <param name="kvp">The KeyValuePair to use.</param>
    /// <returns>True if the file contains this exact KeyValuePair combination.</returns>
    public bool Contains(KeyValuePair<string, IDictionary<string, string>> kvp) => sects.Contains(kvp);

    /// <summary>
    /// Tries to get a section from this ini file.
    /// </summary>
    /// <param name="sec">The section to get.</param>
    /// <param name="dict">The section's collection of keys=values (if found).</param>
    /// <returns>True if the section exists within this file.</returns>
    public bool TryGetValue(string sec, out IDictionary<string, string> dict) => sects.TryGetValue(sec, out dict);

    /// <summary>
    /// Gets or sets a section's collection of keys=values.
    /// </summary>
    /// <param name="sec">The section's name.</param>
    /// <returns>The section's collection of keys and values.</returns>
    public IDictionary<string, string> this[string sec]
    {
      set => sects[sec] = value;
      get => sects[sec];
    }

    /// <summary>
    /// Gets the collection of section dictionaries.
    /// </summary>
    /// <returns>The collection of section dictionaries.</returns>
    public ICollection<IDictionary<string, string>> Values => sects.Values;

    /// <summary>
    /// Gets the collection of section names in this ini file.
    /// </summary>
    /// <returns>The collection of section names.</returns>
    public ICollection<string> Keys => sects.Keys;

    /// <summary>
    /// Gets the amount of sections in this file.
    /// </summary>
    public int Count => sects.Count;

    /// <summary>
    /// Is this ini file read-only? Returns false by default.
    /// </summary>
    public bool IsReadOnly => false;

    /// <summary>
    /// Gets the enumerator for all sections under this file.
    /// </summary>
    /// <returns>The enumerator for this file's sections.</returns>
    public IEnumerator<KeyValuePair<string, IDictionary<string, string>>> GetEnumerator() => sects.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => sects.GetEnumerator();

    IEnumerable<string> IReadOnlyDictionary<string, IDictionary<string, string>>.Keys => Keys;

    IEnumerable<IDictionary<string, string>> IReadOnlyDictionary<string, IDictionary<string, string>>.Values => Values;


    //
    // PUBLIC
    //

    // METHODS

    /// <summary>
    /// Gets a section if it already exists or creates a new one.
    /// </summary>
    /// <param name="key">The section's name.</param>
    /// <returns>The dictionary representing the section.</returns>
    public virtual IDictionary<string, string> GetOrAddSection(string key) => sects.AddOrGet(key, () => new Dictionary<string, string>());

    /// <summary>
    /// Gets a value out of this ini file. Returns a default value if the key does not exist.
    /// </summary>
    /// <param name="sec">The section name.</param>
    /// <param name="key">The key name.</param>
    /// <param name="def">Default value, if the section or the key aren't located.</param>
    /// <returns>The key's value.</returns>
    public string GetValue(string sec, string key, string def = "")
    {
      if (!TryGetValue(sec, out IDictionary<string, string> dict)) return def;
      if (dict.TryGetValue(key, out string value)) return value;
      return def;
    }

    /// <summary>
    /// Sets a key's value under a section.
    /// </summary>
    /// <param name="sec">The section. It will be automatically created if it does not exist.</param>
    /// <param name="key">The key. It will be automatically created if it does not exist.</param>
    /// <param name="value">The value to set the key to.</param>
    public void SetValue(string sec, string key, string value) => GetOrAddSection(sec).AddOrSet(key, value);

    //
    // PRIVATE
    //

    // VARIABLES

    private readonly IDictionary<string, IDictionary<string, string>> sects;
  }
}