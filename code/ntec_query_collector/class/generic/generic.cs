using System.Web;
using System.Web.UI;
using System;
using System.Collections;
using System.Xml;

/// <summary>
/// Methods that can be used in all pages.
/// </summary>
public class Generic
{
    /// <summary>
    /// Remove spaces and tabs from string.
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    static public string RemoveUnwantedChars(string data)
    {
        data = data.Replace("\r\n", " ");
        data = data.Replace("\r\n", " ");
        data = data.Replace("\t", " ");
        data = data.Replace("\n", " ");

        return data.Trim();
    }

    static public string RemoveStartEndSquareBrackets(string data)
    {
        try
        {
            data = data.Trim();

            if (data.StartsWith("["))
                data = data.Substring(1);

            if (data.EndsWith("]"))
                data = data.Substring(0, data.Length - 1);

        }
        catch { }

        return data.Trim();
    }

}