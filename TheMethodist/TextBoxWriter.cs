using System.Text;
using System.Windows.Forms;
using System.IO;

namespace TheMethodist
{
    internal sealed class TextBoxWriter : TextWriter, IWriter
    {
        private TextBox _textBox;

        internal TextBoxWriter(TextBox textBox)
        {
            _textBox = textBox;
        }

        public override Encoding Encoding
        {
            get { return Encoding.Default; }
        }

        public override void Write(string value)
        {
            _textBox.AppendText(value.Replace("\n", NewLine));
        }

        public override void WriteLine(string value)
        {
            Write(value);
            Write(NewLine);
        }
    }
}
