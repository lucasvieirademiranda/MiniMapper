using System;
using Microsoft.Practices.Unity;

namespace MiniMapper
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var container = ContainerConfiguration.GetConfiguredContainer();

            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
            System.Windows.Forms.Application.Run(container.Resolve<Main>());
        }
    }
}
