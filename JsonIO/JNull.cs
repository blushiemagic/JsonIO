using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonIO
{
    public class JNull : JValue
    {
        private JNull() { }

        public override bool IsNull()
        {
            return true;
        }

        public override void WriteValue(TextWriter writer)
        {
            writer.Write("null");
        }

        public override bool Equals(object obj)
        {
            return obj is JNull;
        }

        public override int GetHashCode()
        {
            return -1;
        }

        public static readonly JNull Value = new JNull();
    }
}
