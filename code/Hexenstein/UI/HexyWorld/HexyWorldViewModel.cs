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

            Visuals = new ReactiveCollection<Visual3D>();

            Visuals.Add(new SunLight());

            Visuals.Add(hexy);
        }

        public ReactiveCollection<Visual3D> Visuals { get; set; }

        public double Value { get; set; }

        private void OnValueChanged()
        {
            hexy.SetValue(Value);
        }
    }
}