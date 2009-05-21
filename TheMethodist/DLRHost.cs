using System;
using System.Collections.Generic;
using Microsoft.Scripting.Hosting;
using System.Reflection;
using Microsoft.Scripting;
using System.IO;
using IronEditor.Engine;
using System.Threading;

namespace TheMethodist
{
   public class DLRHost : IExecute, ILoad
   {
      public IServiceProvider ServiceProvider { get; set;}
      ScriptEngine Engine;
      ScriptScope Scope;

      public DLRHost(IServiceProvider serviceProvider)
      {
         ServiceProvider = serviceProvider;

         IDictionary<string, object> options = new Dictionary<string, object>();
         options.Add("PrivateBinding", true);
         ScriptEngine engine = IronPython.Hosting.Python.CreateEngine(options);
         
         SetEngine(engine);
      }

      public void SetEngine(ScriptEngine engine)
      {
         Engine = engine;
         Scope = Engine.CreateScope();
         
         Scope.SetVariable("v", "1.0.0.0");
         Scope.SetVariable("this", this);
         Execute(@"def printText(object):
                        print object.Text");
      }

      public void RedirectOut(TextWriter writer)
      {
         TextWriterStream textStreamWriter = new TextWriterStream(writer);

         Engine.Runtime.IO.SetOutput(textStreamWriter, writer);
         Engine.Runtime.IO.SetErrorOutput(textStreamWriter, writer);
         Console.SetOut(writer);
         Console.SetError(writer);
      }

      public string Execute(string code)
      {
         try
         {
            ScriptSource source = Engine.CreateScriptSourceFromString(code, (code.Contains(Environment.NewLine)) ? SourceCodeKind.Statements : SourceCodeKind.InteractiveCode);
            object result = source.Execute(Scope);

            if (result == null)
               return string.Empty;
            else
               return result.ToString();
         }
         catch (Exception ex)
         {
            Console.Error.WriteLine(ex.Message);
            return string.Empty;
         }
      }

      public void Load(Reflector.CodeModel.IAssembly item)
      {
         try
         {
            //%SystemRoot%\Microsoft.net\Framework\v2.0.50727\mscorlib.dll  
            //Engine.Runtime.LoadAssembly(Assembly.LoadFile(Environment.ExpandEnvironmentVariables(item.Location)));
            ThreadStart starter2 = delegate { Engine.Runtime.LoadAssembly(Assembly.LoadFile(Environment.ExpandEnvironmentVariables(item.Location))); };
            new Thread(starter2).Start();
         }
         catch (Exception ex)
         {
            Console.Error.WriteLine("Unable to load assembly: " + item.Location);
         }
      }

      public bool PromptForContinuation(string code)
      {
         ScriptSource s = Engine.CreateScriptSourceFromString(code, SourceCodeKind.Statements);
         if (!IsComplete(s.GetCodeProperties(Engine.GetCompilerOptions())))
            return true;
         else
            return false;
      }

      private bool IsComplete(ScriptCodeParseResult result)
      {
         if (result == ScriptCodeParseResult.IncompleteStatement
            || result == ScriptCodeParseResult.IncompleteToken
            || result == ScriptCodeParseResult.Invalid)
            return false;
         else
            return true;
      }
   }
}