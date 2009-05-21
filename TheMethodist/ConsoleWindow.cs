using System;
using System.Windows.Forms;
using Reflector.CodeModel;

namespace TheMethodist
{
    internal partial class ConsoleWindow : UserControl
    {
        string lineStart = "py> ";
        private bool inContinuation;
        private string continuationMarker = ".";
        private string codeContinuation = string.Empty;
        public IWriter Output;
        IReader Input;
        DLRHost host;
        internal ConsoleWindow(DLRHost host)
        {
            InitializeComponent();
            this.host = host;

            Output = new TextBoxWriter(textBox);
            host.RedirectOut(Output as TextBoxWriter);

            Input = new TextBoxReader(textBox);
            Input.LineRead += Input_LineRead;
            WriteStart();
        }

        void Input_LineRead(string line)
        {
            if (line.StartsWith(lineStart))
            {
                line = line.Replace(lineStart, "");
                Execute(line.Trim());
            }
            if (line.StartsWith(continuationMarker))
            {
               line = line.TrimStart(Convert.ToChar(continuationMarker), ' ');
               Execute(line.Trim());
            }
            if (!inContinuation)
               WriteStart();
        }

        public void WriteStart()
        {
            Output.Write(lineStart);
        }

       public void WriteContinuation()
       {
          Output.Write(continuationMarker.PadRight(7, ' '));
       }

        public void Execute(string code)
        {
          codeContinuation += code;

           if (host.PromptForContinuation(codeContinuation))
           {
              inContinuation = true;
              WriteContinuation();
              
           }
           else
           {
              
              inContinuation = false;
              host.Execute(codeContinuation);
              codeContinuation = string.Empty;
           }
        }

       public void WriteImport(INamespace item)
       {
          if (item == null) return;
          Output.Write(string.Format("from {0} import *", item.Name));
       }
    }
}
