using System;
using System.Windows.Forms;
using Reflector;
using Reflector.CodeModel;

namespace TheMethodist
{
   internal class ConsoleWindowManager
   {
      IServiceProvider serviceProvider;
      private readonly DLRHost Host;
      IWindowManager windowManager;
      ConsoleWindow c;

      public ConsoleWindowManager(IServiceProvider service, DLRHost host)
      {
         serviceProvider = service;
         Host = host;
         windowManager = (IWindowManager)serviceProvider.GetService(typeof(IWindowManager));
      }

      private void DisplayConsoleWindow(object sender, EventArgs e)
      {
         SetWindowVisability(true);
      }

      private void SetWindowVisability(bool show)
      {
         windowManager.Windows["Console"].Visible = show;
      }

      public void AddToolbarItem()
      {
         var commandBarManager = (ICommandBarManager)serviceProvider.GetService(typeof(ICommandBarManager));
         commandBarManager.CommandBars["Tools"].Items.AddSeparator();

         commandBarManager.CommandBars["Tools"].Items.AddButton("The Methodist", new EventHandler(DisplayConsoleWindow), Keys.Control | Keys.D);
      }

      public void AddContextMenu()
      {
         var commandBarManager = (ICommandBarManager)serviceProvider.GetService(typeof(ICommandBarManager));
         ICommandBarButton subMenu = commandBarManager.CommandBars["Browser.Namespace"].Items.AddButton("Import into The Methodist", importIntoDLR);
      }

      private void importIntoDLR(object sender, EventArgs e)
      {
         if (c != null)
         {
            var assemblyBrowser = (IAssemblyBrowser)serviceProvider.GetService(typeof(IAssemblyBrowser));

            c.WriteImport(assemblyBrowser.ActiveItem as INamespace);
            SetWindowVisability(true);
         }
      }

      public void CreateConsoleWindow()
      {
         c = new ConsoleWindow(Host);

         windowManager.Windows.Add("Console", c, "The Methodist");
      }
   }
}
