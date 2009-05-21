using System;
using System.Collections.Generic;
using Reflector.CodeModel;

namespace TheMethodist
{
   public class AssemblyLoader
   {
      readonly IAssemblyManager AssemblyManager;
      readonly ILoad Dlr;
      readonly List<IAssembly> LoadedAssemblies;

      public AssemblyLoader(IAssemblyManager manager, ILoad dlr)
      {
         Dlr = dlr;
         AssemblyManager = manager;
         AssemblyManager.AssemblyLoaded += assemblyManager_AssemblyLoaded;
         LoadedAssemblies = new List<IAssembly>();
      }

      public void LoadAssemblies()
      {
         foreach (IAssembly item in AssemblyManager.Assemblies)
         {
            LoadNewAssembly(item);
         }
      }

      private void LoadNewAssembly(IAssembly item)
      {
         Dlr.Load(item);
         LoadedAssemblies.Add(item);
      }

      void assemblyManager_AssemblyLoaded(object sender, EventArgs e)
      {
         foreach (IAssembly item in AssemblyManager.Assemblies)
         {
            if (!LoadedAssemblies.Contains(item))
            {
               LoadNewAssembly(item);
            }
         }
      }
   }
}
