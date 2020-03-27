using Dapper;
using OMI.Core.Security.Crypt;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace OMI.Core.Helper
{
  /// <summary>
  ///   Cache Helper for all class implement cache
  /// </summary>
  public static class CacheHelper
  {
    /// <summary>
    ///   Generates the key cache.
    /// </summary>
    /// <returns></returns>
    public static string GenerateKeyCache()
    {
      var stackTrace = new StackTrace(true);
      var methodBase = stackTrace.GetFrame(1).GetMethod();
      var type = methodBase.ReflectedType;
      var className = type.Name;
      var key = string.Format("dbo-{1}_{2}", className, methodBase.Name);
      
      return Md5Util.Md5EnCrypt(key);
    }

    /// <summary>
    ///   Generates the key cache.
    /// </summary>
    /// <param name="oParams">The o params.</param>
    /// <returns></returns>
    public static string GenerateSuffixKeyByParams(object oParams)
    {
      var key = "";
      if (oParams == null) return key;
      
      var properties = oParams.GetType().GetProperties();
      foreach (var propertyInfo in properties)
      {
        var attributes = propertyInfo.GetCustomAttributes(false);
        if (propertyInfo.CanRead && !propertyInfo.GetMethod.IsVirtual
                                 && !attributes.Any(a => a is KeyAttribute)
                                 && !attributes.Any(a => a is NotMappedAttribute)
        )
        {
          var value = propertyInfo.GetValue(oParams, null);
          var name = propertyInfo.Name;
          key += string.Format("_{0}_{1}", name, value);
        }
      }

      return key;
    }
  }
}
