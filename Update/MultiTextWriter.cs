using System.Text;

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
                writer.WriteLine($"[{getCurrentTime()}] {value}");
        }

        public override void Write(char value)
        {
            foreach (var writer in writers)
                writer.Write($"{value}");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (var writer in writers)
                {
                    writer.Flush();
                    writer.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        private string getCurrentTime()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}
