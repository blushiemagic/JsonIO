using System;
using System.Collections.Generic;
using System.IO;

namespace JsonIO
{
    public class JList : JValue
    {
        private List<JValue> value;

        public JList(List<JValue> value)
        {
            this.value = value;
        }
        
        public JList()
        {
            this.value = new List<JValue>();
        }

        public override JValue this[int index]
        {
            get { return this.value[index]; }
            set { this.value[index] = value; }
        }

        public override bool Add(JValue value)
        {
            return value.Add(value);
        }

        public override bool Add(JValue value, int index)
        {
            return value.Add(value, index);
        }

        public override void Remove(int index)
        {
            value.RemoveAt(index);
        }

        public override IEnumerable<JValue> EnumerateList()
        {
            return value;
        }

        public override void WriteValue(TextWriter writer)
        {
            writer.Write('[');
            for (int k = 0; k < value.Count; k++)
            {
                value[k].WriteValue(writer);
                if (k < value.Count - 1)
                {
                    writer.Write(", ");
                }
            }
            writer.Write(']');
        }

        public override bool Equals(object obj)
        {
            if (!(obj is JList))
            {
                return false;
            }
            List<JValue> other = ((JList)obj).value;
            if (value.Count != other.Count)
            {
                return false;
            }
            for (int k = 0; k < value.Count; k++)
            {
                if (!value[k].Equals(other[k]))
                {
                    return false;
                }
            }
            return true;
        }

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }
    }
}
