using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class EventAggregator
    {
        public event EventHandler<BlEventArgs>? BlEvent;

        internal void FireEvent(BlEvents theEvent)
        {
            BlEvent?.Invoke(this, new BlEventArgs(theEvent));
        }
    }

    public class BlEventArgs : EventArgs
    {
        public BlEventArgs(BlEvents theEvent)
        {
            BlEvent = theEvent;
        }

        public BlEvents BlEvent { get; private set; }
}
}
