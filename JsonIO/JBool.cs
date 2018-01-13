using System;
using System.IO;

namespace JsonIO
{
    public class JBool : JValue
    {
        private bool value;

        private JBool(bool value)
        {
            this.value = value;
        }

        public override bool GetBool()
        {
            return value;
        }

        public override void WriteValue(TextWriter writer)
        {
            if (value)
            {
                writer.Write("true");
            }
            else
            {
                writer.Write("false");
            }
        }

        public override bool Equals(object obj)
        {
            return obj is JBool && ((JBool)obj).value == value;
        }

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        public static readonly JBool True = new JBool(true);
        public static readonly JBool False = new JBool(false);
    }
}
