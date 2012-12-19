using System.Reflection;
using Hexenstein.Framework.Reactive;
using Hexenstein.UI.ControlPanel;
using Hexenstein.UI.HexyWorld;

namespace Hexenstein.UI.Shell
{
    internal class ShellViewModel : ReactiveScreen
    {
        public ShellViewModel(ControlPanelViewModel controlPanel, HexyWorldViewModel hexyWorld)
        {
            ControlPanel = controlPanel;
            HexyWorld = hexyWorld;

            var info = (AssemblyInformationalVersionAttribute)Assembly.GetAssembly(typeof(ShellViewModel)).GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute), false)[0];

            DisplayName = "Hexenstein " + info.InformationalVersion;
        }

        public ControlPanelViewModel ControlPanel { get; set; }

        public HexyWorldViewModel HexyWorld { get; set; }
    }
}