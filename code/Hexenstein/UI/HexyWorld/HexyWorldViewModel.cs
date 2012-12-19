using Hexenstein.Emulator;
using Hexenstein.Framework.Reactive;

namespace Hexenstein.UI.HexyWorld
{
    internal class HexyWorldViewModel : ReactiveScreen
    {
        private readonly Hexy hexy;

        public HexyWorldViewModel(Hexy hexy)
        {
            this.hexy = hexy;
        }
    }
}