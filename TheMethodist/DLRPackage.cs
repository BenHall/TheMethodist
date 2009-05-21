using System;
using Reflector;
using Reflector.CodeModel;

namespace TheMethodist
{
    public class DLRPackage : IPackage
    {
        IServiceProvider ServiceProvider;

       public void Load(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            DLRHost host = new DLRHost(ServiceProvider);

            ConsoleWindowManager manager = new ConsoleWindowManager(serviceProvider, host);

            manager.CreateConsoleWindow();
            manager.AddToolbarItem();
           manager.AddContextMenu();

            AssemblyLoader assembly = new AssemblyLoader((IAssemblyManager)ServiceProvider.GetService(typeof(IAssemblyManager)), host);
            assembly.LoadAssemblies();
        }

       public void Unload()
        {
        }
    }
}
