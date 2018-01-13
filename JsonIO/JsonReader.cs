using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace JsonIO
{
    public static class JsonReader
    {
        public static JValue ReadJson(string json)
        {
            return ReadJson(new StringReader(json));
        }

        public static JValue ReadJson(TextReader reader)
        {
            while (reader.Peek() >= 0)
            {
                char peek = (char)reader.Peek();
                if (!char.IsWhiteSpace(peek))
                {
                    if (peek == '{')
                    {
                        return ReadObject(reader);
                    }
                    else if (peek == '[')
                    {
                        return ReadList(reader);
                    }
                    else if (peek == '"')
                    {
                        return ReadString(reader);
                    }
                    else if (peek == '-' || (peek >= '0' && peek <= '9'))
                    {
                        return ReadNumber(reader);
                    }
                    else
                    {
                        string value = ReadConstant(reader);
                        if (value == "true")
                        {
                            return JBool.True;
                        }
                        else if (value == "false")
                        {
                            return JBool.False;
                        }
                        else if (value == "null")
                        {
                            return JNull.Value;
                        }
                        else
                        {
                            throw new FormatException(string.Format("Unknown value: {0}", value));
                        }
                    }
                }
            }
            throw new FormatException("No JSON value found");
        }

        public static JObject ReadObject(TextReader reader)
        {
            if (reader.Read() != '{')
            {
                throw new FormatException("Object must begin with {");
            }
            Dictionary<string, JValue> value = new Dictionary<string, JValue>();
            string key = null;
            bool valueReady = false;
            bool valueFinished = false;
            bool valueRequired = false;
            while (reader.Peek() >= 0)
            {
                char peek = (char)reader.Peek();
                if (!char.IsWhiteSpace(peek))
                {
                    if (key == null)
                    {
                        if (peek == '"')
                        {
                            key = ReadString(reader).GetString();
                        }
                        else if (!valueRequired && peek == '}')
                        {
                            reader.Read();
                            return new JObject(value);
                        }
                        else
                        {
                            throw new FormatException("Object must have string keys");
                        }
                    }
                    else if (!valueReady)
                    {
                        if (peek == ':')
                        {
                            reader.Read();
                            valueReady = true;
                        }
                        else
                        {
                            throw new FormatException(": must separate a key and value");
                        }
                    }
                    else if (valueFinished)
                    {
                        if (peek == '}')
                        {
                            reader.Read();
                            return new JObject(value);
                        }
                        else if (peek == ',')
                        {
                            reader.Read();
                            key = null;
                            valueReady = false;
                            valueFinished = false;
                            valueRequired = true;
                        }
                        else
                        {
                            throw new FormatException(", must be used to separate key-value pairs of object");
                        }
                    }
                    else
                    {
                        value[key] = ReadJson(reader);
                        valueFinished = true;
                    }
                }
            }
            throw new FormatException("Objects must end with }");
        }

        public static JList ReadList(TextReader reader)
        {
            if (reader.Read() != '[')
            {
                throw new FormatException("List must begin with [");
            }
            List<JValue> values = new List<JValue>();
            bool valueReady = true;
            bool valueRequired = false;
            while (reader.Peek() >= 0)
            {
                char peek = (char)reader.Peek();
                if (valueReady)
                {
                    if (!valueRequired && peek == ']')
                    {
                        reader.Read();
                        return new JList(values);
                    }
                    else if (peek == ']' || peek == ',')
                    {
                        throw new FormatException("A , must separate two values");
                    }
                    else
                    {
                        values.Add(ReadJson(reader));
                        valueReady = false;
                    }
                }
                else if (peek == ',')
                {
                    reader.Read();
                    valueReady = true;
                    valueRequired = true;
                }
                else if (peek == ']')
                {
                    reader.Read();
                    return new JList(values);
                }
                else
                {
                    throw new FormatException("Two values must be separated by a ,");
                }
            }
            throw new FormatException("Arrays must end with ]");
        }

        public static JString ReadString(TextReader reader)
        {
            if (reader.Read() != '"')
            {
                throw new FormatException("String must begin with \"");
            }
            List<char> values = new List<char>();
            bool escape = false;
            string hex = null;
            while (reader.Peek() >= 0)
            {
                char peek = (char)reader.Peek();
                if (hex != null)
                {
                    if ((peek >= '0' && peek <= '9') || (peek >= 'a' && peek <= 'f'))
                    {
                        hex += peek;
                    }
                    else if (peek >= 'A' && peek <= 'F')
                    {
                        hex += peek - 'A' + 'a';
                    }
                    else
                    {
                        throw new FormatException("Invalid sequence following \\");
                    }
                    reader.Read();
                    if (hex.Length == 4)
                    {
                        values.Add((char)Int16.Parse(hex, NumberStyles.AllowHexSpecifier));
                        hex = null;
                    }
                }
                else if (escape)
                {
                    switch (peek)
                    {
                        case '"':
                            values.Add('"');
                            break;
                        case '\\':
                            values.Add('\\');
                            break;
                        case '/':
                            values.Add('/');
                            break;
                        case 'b':
                            values.Add('\b');
                            break;
                        case 'f':
                            values.Add('\f');
                            break;
                        case 'n':
                            values.Add('\n');
                            break;
                        case 'r':
                            values.Add('\r');
                            break;
                        case 't':
                            values.Add('\t');
                            break;
                        case 'u':
                            hex = "";
                            break;
                        default:
                            throw new FormatException("Invalid sequence following \\");
                    }
                    reader.Read();
                    escape = false;
                }
                else if (peek == '"')
                {
                    reader.Read();
                    return new JString(new string(values.ToArray()));
                }
                else if (peek == '\\')
                {
                    reader.Read();
                    escape = true;
                }
                else if (char.IsControl(peek))
                {
                    throw new FormatException("Strings may not directly contain control characters");
                }
                else
                {
                    values.Add(peek);
                    reader.Read();
                }
            }
            throw new FormatException("Strings must end with \"");
        }

        public static JValue ReadNumber(TextReader reader)
        {
            bool negative = false;
            if (reader.Peek() == '-')
            {
                negative = true;
                reader.Read();
            }
            double value = 0;
            if (reader.Peek() < 0)
            {
                throw new FormatException("Unknown value");
            }
            char peek = (char)reader.Peek();
            if (peek == '0')
            {
                reader.Read();
            }
            else if (peek >= '1' && peek <= '9')
            {
                while (peek >= '0' && peek <= '9')
                {
                    value *= 10;
                    value += peek - '0';
                    reader.Read();
                    if (reader.Peek() < 0)
                    {
                        break;
                    }
                    peek = (char)reader.Peek();
                }
            }
            else
            {
                throw new FormatException("Unknown value");
            }

            if (reader.Peek() == '.')
            {
                reader.Read();
                if (reader.Peek() < 0)
                {
                    throw new FormatException("A . must be followed by digits");
                }
                peek = (char)reader.Peek();
                if (peek < '0' || peek > '9')
                {
                    throw new FormatException("A . must be followed by digits");
                }
                double multiplier = 0.1;
                while (peek >= '0' && peek <= '9')
                {
                    value += (peek - '0') * multiplier;
                    multiplier *= 0.1;
                    reader.Read();
                    if (reader.Peek() < 0)
                    {
                        break;
                    }
                    peek = (char)reader.Peek();
                }
            }

            if (reader.Peek() == 'e' || reader.Peek() == 'E')
            {
                reader.Read();
                if (reader.Peek() < 0)
                {
                    throw new FormatException("The exponent specifier must be followed by a + or - or digit");
                }
                peek = (char)reader.Peek();
                double exponent = 0;
                bool exponentNegative = peek == '-';
                if (peek == '+' || peek == '-')
                {
                    reader.Read();
                    if (reader.Peek() < 0)
                    {
                        throw new FormatException("The exponent must have digits");
                    }
                    peek = (char)reader.Peek();
                }
                if (peek < '0' || peek > '9')
                {
                    throw new FormatException("The exponent must have digits");
                }
                while (peek >= '0' && peek <= '9')
                {
                    exponent *= 10;
                    exponent += peek - '0';
                    reader.Read();
                    if (reader.Peek() < 0)
                    {
                        break;
                    }
                    peek = (char)reader.Peek();
                }
                if (exponentNegative)
                {
                    exponent *= -1;
                }
                value *= Math.Pow(10, exponent);
            }

            if (negative)
            {
                value *= -1;
            }
            try
            {
                int intValue = (int)value;
                if (value == intValue)
                {
                    return new JInt(intValue);
                }
            }
            catch
            {
            }
            return new JFloatingPoint(value);
        }

        public static string ReadConstant(TextReader reader)
        {
            List<char> values = new List<char>();
            while (reader.Peek() >= 'a' && reader.Peek() <= 'z')
            {
                values.Add((char)reader.Read());
            }
            return new string(values.ToArray());
        }
    }
}
