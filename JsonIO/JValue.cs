using System;
using System.Collections.Generic;
using System.IO;

namespace JsonIO
{
    public abstract class JValue
    {
        public virtual string GetString()
        {
            throw new NotImplementedException();
        }

        public virtual int GetInt()
        {
            throw new NotImplementedException();
        }

        public virtual float GetSingle()
        {
            throw new NotImplementedException();
        }

        public virtual double GetDouble()
        {
            throw new NotImplementedException();
        }

        public virtual bool GetBool()
        {
            throw new NotImplementedException();
        }

        public virtual JValue this[int index]
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public virtual bool Add(JValue value)
        {
            throw new NotImplementedException();
        }

        public virtual bool Add(JValue value, int index)
        {
            throw new NotImplementedException();
        }

        public virtual void Remove(int index)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<JValue> EnumerateList()
        {
            throw new NotImplementedException();
        }

        public virtual JValue this[string key]
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public virtual bool ContainsKey(string key)
        {
            throw new NotImplementedException();
        }

        public virtual bool Remove(string key)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<KeyValuePair<string, JValue>> EnumerateObject()
        {
            throw new NotImplementedException();
        }

        public virtual bool IsNull()
        {
            return false;
        }

        public abstract void WriteValue(TextWriter writer);

        public static bool operator ==(JValue val1, JValue val2)
        {
            return val1.Equals(val2);
        }

        public static bool operator !=(JValue val1, JValue val2)
        {
            return !val1.Equals(val2);
        }

        public override string ToString()
        {
            StringWriter writer = new StringWriter();
            WriteValue(writer);
            return writer.ToString();
        }
    }
}
