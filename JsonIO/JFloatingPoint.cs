using System;
using System.IO;

namespace JsonIO
{
    public class JFloatingPoint : JValue
    {
        private double value;

        public JFloatingPoint(double value)
        {
            this.value = value;
        }

        public override float GetSingle()
        {
            return (float)value;
        }

        public override double GetDouble()
        {
            return value;
        }

        public override void WriteValue(TextWriter writer)
        {
            writer.Write(value);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj, 1E-7);
        }

        public bool Equals(object obj, double epsilon)
        {
            double other;
            if (obj is JInt)
            {
                other = ((JInt)obj).GetDouble();
            }
            else  if (obj is JFloatingPoint)
            {
                other = ((JFloatingPoint)obj).value;
            }
            else
            {
                return false;
            }
            epsilon = Math.Min(Math.Abs(value), Math.Abs(other)) * epsilon;
            return Math.Abs(value - other) <= epsilon;
        }

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }
    }
}
