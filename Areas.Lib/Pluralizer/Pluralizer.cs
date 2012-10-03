using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
public static class Formatting
{
    // Fields
    private static readonly IDictionary<string, string> Pluralizations;
    private static readonly IList<string> Unpluralizables;

    // Methods
    static Formatting()
    {
        List<string> list = new List<string>();
        list.Add("equipment");
        list.Add("information");
        list.Add("rice");
        list.Add("money");
        list.Add("species");
        list.Add("series");
        list.Add("fish");
        list.Add("sheep");
        list.Add("deer");
        Unpluralizables = list;
        Dictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary.Add("person", "people");
        dictionary.Add("ox", "oxen");
        dictionary.Add("child", "children");
        dictionary.Add("foot", "feet");
        dictionary.Add("tooth", "teeth");
        dictionary.Add("goose", "geese");
        dictionary.Add("(.*)fe?", "$1ves");
        dictionary.Add("(.*)man$", "$1men");
        dictionary.Add("(.+[aeiou]y)$", "$1s");
        dictionary.Add("(.+[^aeiou])y$", "$1ies");
        dictionary.Add("(.+z)$", "$1zes");
        dictionary.Add("([m|l])ouse$", "$1ice");
        dictionary.Add("(.+)(e|i)x$", "$1ices");
        dictionary.Add("(octop|vir)us$", "$1i");
        dictionary.Add("(.+(s|x|sh|ch))$", "$1es");
        dictionary.Add("(.+)", "$1s");
        Pluralizations = dictionary;
    }

    public static string Pluralize(this string noun)
    {
        if (Singularizer.IsPlural(noun))
            return noun;
        if (Unpluralizables.Contains(noun))
        {
            return noun;
        }

        foreach (KeyValuePair<string, string> pair in Pluralizations)
        {
            if (Regex.IsMatch(noun, pair.Key))
            {
                return Regex.Replace(noun, pair.Key, pair.Value);
            }
        }
        return "";
    }
}
public static class Singularizer
{
    // Fields
    private static IDictionary<string, string> Singularizations;
    private static IList<string> Unpluralizables;

    // Methods
    static Singularizer()
    {
        List<string> list = new List<string>();
        list.Add("equipment");
        list.Add("information");
        list.Add("rice");
        list.Add("money");
        list.Add("species");
        list.Add("series");
        list.Add("fish");
        list.Add("sheep");
        list.Add("deer");
        Unpluralizables = list;
        Dictionary<string, string> dictionary = new Dictionary<string, string>();
        dictionary.Add("people", "person");
        dictionary.Add("oxen", "ox");
        dictionary.Add("children", "child");
        dictionary.Add("feet", "foot");
        dictionary.Add("teeth", "tooth");
        dictionary.Add("geese", "goose");
        dictionary.Add("(.*)ives?", "$1ife");
        dictionary.Add("(.*)ves?", "$1f");
        dictionary.Add("(.*)men$", "$1man");
        dictionary.Add("(.+[aeiou])ys$", "$1y");
        dictionary.Add("(.+[^aeiou])ies$", "$1y");
        dictionary.Add("(.+)zes$", "$1");
        dictionary.Add("([m|l])ice$", "$1ouse");
        dictionary.Add("matrices", "matrix");
        dictionary.Add("indices", "index");
        dictionary.Add("(.*)ices", "$1ex");
        dictionary.Add("(octop|vir)i$", "$1us");
        dictionary.Add("(.+(s|x|sh|ch))es$", "$1");
        dictionary.Add("(.+)s", "$1");
        Singularizations = dictionary;
    }

    public static bool IsPlural(this string word)
    {
        if (Unpluralizables.Contains(word.ToLowerInvariant()))
        {
            return true;
        }
        foreach (KeyValuePair<string, string> pair in Singularizations)
        {
            if (Regex.IsMatch(word, pair.Key))
            {
                return true;
            }
        }
        return false;
    }

    public static string Singularize(this string word)
    {
        if (!Unpluralizables.Contains(word.ToLowerInvariant()))
        {
            foreach (KeyValuePair<string, string> pair in Singularizations)
            {
                if (Regex.IsMatch(word, pair.Key))
                {
                    return Regex.Replace(word, pair.Key, pair.Value);
                }
            }
        }
        return word;
    }
}
