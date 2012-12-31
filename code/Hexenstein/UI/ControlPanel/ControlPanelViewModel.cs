using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Ports;
using Caliburn.Micro;
using Hexenstein.Emulator;

namespace Hexenstein.UI.ControlPanel
{
    internal class ControlPanelViewModel : Screen
    {
        private readonly ServotorEmulator hexy;

        public ControlPanelViewModel(ServotorEmulator hexy)
        {
            this.hexy = hexy;
            string[] portName = SerialPort.GetPortNames();
            COMPorts = new ObservableCollection<string>(portName);
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