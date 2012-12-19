using System.Reflection;
using Hexenstein.Framework.Reactive;
using Hexenstein.UI.ControlPanel;

namespace Hexenstein.UI.Shell
{
    internal class ShellViewModel : ReactiveScreen
    {
        public ShellViewModel(ControlPanelViewModel controlPanel)
        {
            ControlPanel = controlPanel;

            var info = (AssemblyInformationalVersionAttribute)Assembly.GetAssembly(typeof(ShellViewModel)).GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute), false)[0];

            DisplayName = "Hexenstein " + info.InformationalVersion;
        }

        public ControlPanelViewModel ControlPanel { get; set; }
    }
}