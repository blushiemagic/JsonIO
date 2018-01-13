using System;
using System.IO;

namespace JsonIO
{
    public class JString : JValue
    {
        private string value;

        public JString(string value)
        {
            this.value = value;
        }

        public override string GetString()
        {
            return value;
        }

        public override void WriteValue(TextWriter writer)
        {
            for (int k = 0; k < value.Length; k++)
            {
                switch (value[k])
                {
                    case '"':
                        writer.Write("\\\"");
                        break;
                    case '\\':
                        writer.Write("\\\\");
                        break;
                    case '/':
                        writer.Write("\\/");
                        break;
                    case '\b':
                        writer.Write("\\b");
                        break;
                    case '\f':
                        writer.Write("\\f");
                        break;
                    case '\n':
                        writer.Write("\\n");
                        break;
                    case '\r':
                        writer.Write("\\r");
                        break;
                    case '\t':
                        writer.Write("\\t");
                        break;
                    default:
                        if (char.IsControl(value[k]))
                        {
                            writer.Write("\\u");
                            int intValue = (int)value[k];
                            writer.Write(intValue.ToString("X4"));
                        }
                        else
                        {
                            writer.Write(value[k]);
                        }
                        break;
                }
            }
        }

        public override bool Equals(object obj)
        {
            return obj is JString && value == ((JString)obj).value;
        }

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }
    }
}
