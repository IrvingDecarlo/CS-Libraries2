using System;
using System.Diagnostics;
using System.Reflection;

namespace Cephei.Assemblies
{
  /// <summary>
  /// The AssemblyExtensions class contains extensions that refer mostly to Assemblies.
  /// </summary>
  public static class AssemblyExtensions
  {
    /// <summary>
    /// Gets the assembly's FileVersionInfo using its location.
    /// </summary>
    /// <param name="assembly">Assembly to get the data from.</param>
    /// <returns>The assembly's FileVersionInfo.</returns>
    public static FileVersionInfo GetFileVersionInfo(this Assembly assembly) => FileVersionInfo.GetVersionInfo(assembly.Location);

    /// <summary>
    /// Gets a new version object using a FileVersionInfo object.
    /// </summary>
    /// <param name="file">The file object.</param>
    /// <returns>The new version object based on the FileVersionInfo object.</returns>
    public static Version ToVersion(this FileVersionInfo file) => new Version(file.FileMajorPart, file.FileMinorPart, file.FileBuildPart, file.FilePrivatePart);

    /// <summary>
    /// Returns a DynamicString containing most of the file's relevant data.
    /// </summary>
    /// <param name="file">The FileVersionInfo representing the target file.</param>
    /// <param name="company">Show the file's company?</param>
    /// <param name="minv">Will at least show this many version fields.</param>
    /// <param name="maxv">Will at most show this many version fields.</param>
    /// <returns>A string with the file's most relevant data.</returns>
    public static string ToDynamicString(this FileVersionInfo file, bool company = true, byte minv = 1, byte maxv = 4)
        => $"{file.ProductName} (" + (company ? $"{file.CompanyName}, " : "") + $"v{file.ToVersion().ToDynamicString(minv, maxv)})";
    /// <summary>
    /// Returns a string representing an assembly, returning its version.
    /// </summary>
    /// <param name="assembly">The assembly.</param>
    /// <param name="repspaces">Replace dots and underlines with spaces?</param>
    /// <param name="showver">Show version identifier 'v'?</param>
    /// <param name="minv">Will at least show this many version fields.</param>
    /// <param name="maxv">Will at most show this many version fields.</param>
    /// <returns>The assembly to string.</returns>
    public static string ToDynamicString(this Assembly assembly, bool repspaces = true, bool showver = true, byte minv = 1, byte maxv = 4)
        => assembly.GetName().ToDynamicString(repspaces, showver, minv, maxv);
    /// <summary>
    /// Returns a string representing an assembly, returning its version.
    /// </summary>
    /// <param name="assembly">The assembly.</param>
    /// <param name="repspaces">Replace dots and underlines with spaces?</param>
    /// <param name="showver">Show version identifier 'v'?</param>
    /// <param name="minv">Will at least show this many version fields.</param>
    /// <param name="maxv">Will at most show this many version fields.</param>
    /// <returns>The assembly to string.</returns>
    public static string ToDynamicString(this AssemblyName assembly, bool repspaces = true, bool showver = true, byte minv = 1, byte maxv = 4)
        => (repspaces ? assembly.Name.Replace('.', ' ').Replace('_', ' ') : assembly.Name) + " " + (showver ? "v" : "") + assembly.Version.ToDynamicString(minv, maxv);
    /// <summary>
    /// A dynamic version of Version.ToString, where only the real versions (with values higher than 0) are shown, from minv to maxv.
    /// </summary>
    /// <param name="version">The version class.</param>
    /// <param name="minv">Minimum number of version numbers to show.</param>
    /// <param name="maxv">Maximum number of version numbers to show.</param>
    /// <returns>A string representing the version.</returns>
    public static string ToDynamicString(this Version version, byte minv, byte maxv)
    {
      byte v = minv;
      for (byte i = minv; i < maxv; i++)
      {
        if (i == 2 && version.Minor < 1) continue;
        if (i == 3 && version.Build < 1) continue;
        if (i == 4 && version.Revision < 1) continue;
        v = i;
      }
      return version.ToString(v);
    }
  }
}
