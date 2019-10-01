using System;
using System.Collections.Generic;
using MineLW.Debugging;
using MineLW.Serialization.Text;

namespace MineLW.Serialization.Json
{
    public static class JParser
    {
        private static readonly Logger Logger = LogManager.GetLogger(typeof(JParser));

        public static JElement Parse(string json)
        {
            if (string.IsNullOrEmpty(json))
                throw new FormatException("Empty json");

            var reader = new StringReader(json);
            return ParseElement(reader);
        }

        private static JElement ParseElement(StringReader reader)
        {
            var token = reader.Read();
            if (token == '{')
            {
                reader.ConsumeWhitespaces();

                if (reader.CanRead() && reader.Next() == '}')
                    return new JObject();

                var dictionary = new Dictionary<string, JElement>();
                while (reader.CanRead())
                {
                    var key = reader.ReadQuotedString();
                    //Logger.Debug("Key read: \"{0}\"", key);

                    reader.ConsumeWhitespaces();
                    reader.EnsureNext(':');

                    reader.ConsumeWhitespaces();
                    dictionary[key] = ParseElement(reader);

                    //Logger.Debug("JObject value with key \"{0}\" read: \"{1}\" (line: \"{2}\")", key, dictionary[key], reader.Line);
                    //Logger.Debug("JObject with {0} elements", dictionary.Count);

                    reader.ConsumeWhitespaces();
                    var c = reader.Read();
                    if (c == '}')
                        return new JObject(dictionary);
                    if (c != ',')
                        break;

                    reader.ConsumeWhitespaces();
                }

                throw new FormatException("Invalid JObject");
            }

            if (token == '[')
            {
                reader.ConsumeWhitespaces();

                if (reader.CanRead() && reader.Next() == '}')
                    return new JArray();

                var list = new List<JElement>();
                while (reader.CanRead())
                {
                    list.Add(ParseElement(reader));

                    //Logger.Debug("JArray value read: \"{0}\" (line: \"{1}\")", list.Last(), reader.Line);
                    //Logger.Debug("JArray with {0} elements", list.Count);

                    reader.ConsumeWhitespaces();
                    var c = reader.Read();
                    if (c == ']')
                        return new JArray(list);
                    if (c != ',')
                        break;

                    reader.ConsumeWhitespaces();
                }

                throw new FormatException("Invalid JArray");
            }

            if (token == 't')
            {
                reader.EnsureNext('r', 'u', 'e');
                return new JBool(true);
            }

            if (token == 'f')
            {
                reader.EnsureNext('a', 'l', 's', 'e');
                return new JBool(false);
            }

            if (token == 'n')
            {
                reader.EnsureNext('u', 'l', 'l');
                return JNull.Null;
            }

            if (token == '"')
            {
                reader.Position--;
                var str = reader.ReadQuotedString();
                return new JString(str);
            }

            if (token != '-' && !char.IsDigit(token))
                throw new FormatException($"Invalid token (token: \"{token}\",line: \"{reader.Line}\")");

            reader.Position--;
            var number = reader.ReadFloat();
            return new JNumber(number);
        }
    }
}