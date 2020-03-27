using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace OMI.DAL
{
  public static class SqlResource
  {
    /// <summary>
    ///   Get contained SQL resource keys.
    /// </summary>
    /// <returns></returns>
    public static string GetSql(string key)
    {
      // Determine path
      var assembly = Assembly.GetExecutingAssembly();
      string resourcePath = assembly.GetManifestResourceNames()
            .SingleOrDefault(str => str.EndsWith(key + ".sql"));

      if (String.IsNullOrEmpty(resourcePath)) return "";

      using (Stream stream = assembly.GetManifestResourceStream(resourcePath))
      using (StreamReader reader = new StreamReader(stream))
      {
        var s = reader.ReadToEnd();

        var ret = s.Replace("\r\n", " ");
        return ret;
      }
    }
  }
}
