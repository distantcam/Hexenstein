using System.Collections.Generic;
using System.IO.Ports;
using Hexenstein.Emulator;
using Hexenstein.Framework.Reactive;
using ReactiveUI;

namespace Hexenstein.UI.ControlPanel
{
    internal class ControlPanelViewModel : ReactiveScreen
    {
        private readonly Hexy hexy;

        public ControlPanelViewModel(Hexy hexy)
        {
            this.hexy = hexy;
            string[] portName = SerialPort.GetPortNames();
            COMPorts = new ReactiveCollection<string>(portName);
            if (portName.Length > 0)
                SelectedCOMPort = portName[0];
        }

        public IEnumerable<string> COMPorts { get; set; }

        public string SelectedCOMPort { get; set; }

        private void OnSelectedCOMPortChanged()
        {
            hexy.ConnectToPort(SelectedCOMPort);
        }
    }
}