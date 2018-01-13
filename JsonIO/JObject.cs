using System;
using System.Collections.Generic;
using System.IO;

namespace JsonIO
{
    public class JObject : JValue
    {
        private Dictionary<string, JValue> value;

        public JObject(Dictionary<string, JValue> value)
        {
            this.value = value;
        }

        public JObject()
        {
            this.value = new Dictionary<string, JValue>();
        }

        public override JValue this[string key]
        {
            get { return this.value[key]; }
            set { this.value[key] = value; }
        }

        public override bool ContainsKey(string key)
        {
            return value.ContainsKey(key);
        }

        public override bool Remove(string key)
        {
            return value.Remove(key);
        }

        public override IEnumerable<KeyValuePair<string, JValue>> EnumerateObject()
        {
            return value;
        }

        public override void WriteValue(TextWriter writer)
        {
            writer.Write('{');
            bool first = true;
            foreach (KeyValuePair<string, JValue> pair in value)
            {
                if (!first)
                {
                    writer.Write(", ");
                }
                new JString(pair.Key).WriteValue(writer);
                writer.Write(": ");
                pair.Value.WriteValue(writer);
                first = false;
            }
            writer.Write('}');
        }

        public override bool Equals(object obj)
        {
            if (!(obj is JObject))
            {
                return false;
            }
            Dictionary<string, JValue> other = ((JObject)obj).value;
            foreach (string key in value.Keys)
            {
                if (!other.ContainsKey(key) || !value[key].Equals(other[key]))
                {
                    return false;
                }
            }
            foreach (string key in other.Keys)
            {
                if (!value.ContainsKey(key) || !other[key].Equals(value[key]))
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
