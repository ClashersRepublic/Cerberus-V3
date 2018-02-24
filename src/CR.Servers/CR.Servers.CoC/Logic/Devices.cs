using System;
using System.Collections.Concurrent;

namespace CR.Servers.CoC.Logic
{
    public class Devices
    {
        private readonly ConcurrentDictionary<IntPtr, Device> _connected;

        public Devices()
        {
            _connected = new ConcurrentDictionary<IntPtr, Device>();
        }

        public int Count => _connected.Count;

        public void OnConnect(Device device)
        {
            if (_connected.TryAdd(device.Socket.Handle, device))
                Program.UpdateTitle();
        }

        public void OnDisconnect(Device device)
        {
            if (device == null || device.Socket == null)
                return;

            Device _;
            if (_connected.TryRemove(device.Socket.Handle, out _))
                Program.UpdateTitle();
        }
    }
}
