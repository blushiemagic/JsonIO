using System;
using System.IO;

namespace JsonIO
{
    public class JInt : JValue
    {
        private int value;

        public JInt(int value)
        {
            this.value = value;
        }

        public override int GetInt()
        {
            return value;
        }

        public override float GetSingle()
        {
            return value;
        }

        public override double GetDouble()
        {
            return value;
        }

        public override bool Equals(object obj)
        {
            if (obj is JInt)
            {
                return value == ((JInt)obj).value;
            }
            if (obj is JFloatingPoint)
            {
                return obj.Equals(this);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        public override void WriteValue(TextWriter writer)
        {
            writer.Write(value);
        }
    }
}
