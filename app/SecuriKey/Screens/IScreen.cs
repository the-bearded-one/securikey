using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecuriKey.Screens
{
    public interface IScreen
    {
        public event EventHandler<NavigationEventArgs> NavigationRequest;
        public UserControl AsUserControl();
    }

    public class NavigationEventArgs : EventArgs
    {
        public NavigationEventArgs(IScreen screen)
        {
            RequestedScreen = screen;
        }

        public IScreen RequestedScreen { get; private set; }
    }
}
