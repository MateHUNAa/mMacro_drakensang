using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Update
{
    public class MultiTextWriter : TextWriter
    {
        private readonly TextWriter[] writers;
        public override Encoding Encoding => Encoding.UTF8;

        public MultiTextWriter(params TextWriter[] writers)
        {
            this.writers = writers;
        }

        public override void WriteLine(string? value)
        {
            foreach (var writer in writers)
                writer.WriteLine(value);
        }

        public override void Write(char value)
        {
            foreach (var writer in writers)
                writer.Write(value);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (var writer in writers)
                    writer.Flush();
            }
            base.Dispose(disposing);
        }
    }
}
