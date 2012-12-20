using System;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;
using Hexenstein.Emulator;
using Hexenstein.Framework.Reactive;
using ReactiveUI;

namespace Hexenstein.UI.HexyWorld
{
    internal class HexyWorldViewModel : ReactiveScreen
    {
        private readonly ServotorEmulator servotor;

        private Hexy hexy;

        public HexyWorldViewModel(ServotorEmulator servotor)
        {
            this.servotor = servotor;
            hexy = new Hexy();

            servotor.Update += servotor_Update;

            Visuals = new ReactiveCollection<Visual3D>();

            Visuals.Add(new SunLight());

            Visuals.Add(hexy);
        }

        private void servotor_Update(object sender, EventArgs e)
        {
            hexy.SetHip(0, servotor[24]);
            hexy.SetHip(1, servotor[20]);
            hexy.SetHip(2, servotor[16]);
            hexy.SetHip(3, servotor[15]);
            hexy.SetHip(4, servotor[11]);
            hexy.SetHip(5, servotor[7]);
        }

        public ReactiveCollection<Visual3D> Visuals { get; set; }

        public double Value { get; set; }

        private void OnValueChanged()
        {
            hexy.SetValue(Value);
        }
    }
}