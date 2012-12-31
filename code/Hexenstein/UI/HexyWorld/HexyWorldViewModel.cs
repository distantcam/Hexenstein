using System;
using System.Collections.ObjectModel;
using System.Windows.Media.Media3D;
using Caliburn.Micro;
using HelixToolkit.Wpf;
using Hexenstein.Emulator;

namespace Hexenstein.UI.HexyWorld
{
    internal class HexyWorldViewModel : Screen
    {
        private readonly ServotorEmulator servotor;

        private Hexy hexy;

        public HexyWorldViewModel(ServotorEmulator servotor)
        {
            this.servotor = servotor;
            hexy = new Hexy();

            servotor.Update += servotor_Update;

            Visuals = new ObservableCollection<Visual3D>();

            Visuals.Add(new SunLight());

            Visuals.Add(hexy);
        }

        private void servotor_Update(object sender, EventArgs e)
        {
            hexy.Update(servotor.Servos);
        }

        public ObservableCollection<Visual3D> Visuals { get; set; }
    }
}