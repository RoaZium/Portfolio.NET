using System;
using System.Collections.Generic;

namespace PrismWPF.Common.Broadcast
{
    public class BroadcastReceiver
    {
        public delegate void BroadcastEventHandler(string actionName, object sender, object e);
        private readonly Dictionary<string, BroadcastEventHandler> _events = new Dictionary<string, BroadcastEventHandler>();

        private static BroadcastReceiver _instance;
        public static BroadcastReceiver Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new BroadcastReceiver();
                }
                return _instance;
            }
        }

        private BroadcastReceiver() { }

        public static void RegisterReceiver(string actionName, BroadcastEventHandler receiver)
        {
            if (!Instance._events.ContainsKey(actionName))
            {
                BroadcastEventHandler dummy = null;
                Instance._events.Add(actionName, dummy);
            }

            Instance._events[actionName] += receiver;
        }

        public static void UnregisterReceiver(string actionName, BroadcastEventHandler receiver)
        {
            if (Instance._events.ContainsKey(actionName))
            {
                Instance._events[actionName] -= receiver;
            }
        }

        public static void SendBroadcast(string actionName, object sender, object data)
        {
            if (Instance._events.ContainsKey(actionName))
            {
                BroadcastEventHandler receivers = Instance._events[actionName];

                if (receivers != null)
                {
                    receivers(actionName, sender, data);
                }
            }
        }

        public static void ReferenceEquals(string mAP2, object onMapLayoutEditMode)
        {
            throw new NotImplementedException();
        }
    }
}
