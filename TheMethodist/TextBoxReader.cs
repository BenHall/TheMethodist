using System;
using System.Windows.Forms;

namespace TheMethodist
{
    public class TextBoxReader : IReader
    {
        public event Event.ReadLineEventHandler LineRead;

        TextBox TextBox;

       public TextBoxReader(TextBox textBox)
        {
            TextBox = textBox;
            TextBox.TextChanged += TextBox_TextChanged;
            TextBox.KeyDown += TextBox_KeyDown;
            TextBox.MouseClick += TextBox_MouseClick;
        }

        void TextBox_MouseClick(object sender, MouseEventArgs e)
        {
            TextBox.Select(TextBox.Text.Length, 0);
            TextBox.ScrollToCaret();
        }

        void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back)
            {
               if (TextBox.Text.EndsWith("py> ")) //TODO: Wrong location
                    e.SuppressKeyPress = true;
            }
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
                e.SuppressKeyPress = true; //TODO: for now...

            if (e.KeyCode == Keys.Left)
                e.SuppressKeyPress = true;
        }

        void TextBox_TextChanged(object sender, EventArgs e)
        {
            if (TextBox.Text.EndsWith(Environment.NewLine))
                ReadLine();
        }

        public void ReadLine()
        {
           if (TextBox.Lines.Length == 0)
              return;
           string line = TextBox.Lines[TextBox.Lines.Length - 2]; //Get line just entered

            if (LineRead != null && !string.IsNullOrEmpty(line)) LineRead(line);

        }
    }
}
