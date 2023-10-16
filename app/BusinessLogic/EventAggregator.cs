namespace BusinessLogic
{
    public class EventAggregator
    {
        static private EventAggregator _instance = null;

        private EventAggregator()
        {
            // Hide default constructor. Force Singleton pattern
        }

        static internal EventAggregator Instance
        {
            get
            {
                if (_instance == null) _instance = new EventAggregator();
                return _instance;
            }
        }

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
