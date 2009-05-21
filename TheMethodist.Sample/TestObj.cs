using System;

namespace TheMethodist.Sample
{
   public class TestObj
   {
      public void WriteToConsole(string message)
      {
         Console.WriteLine(message);
      }

      private void PrivateMethod()
      {
         Console.WriteLine("hahahaha");
      }

      internal bool Testing()
      {
         return true;
      }
   }
}
