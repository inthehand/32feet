using System;
using System.Collections.Generic;

namespace InTheHand.Net.Bluetooth
{
    public static class BluetoothServiceProtocolMapping
    {
        private static readonly IDictionary<Guid, string> _mapping = new Dictionary<Guid, string>();

        public static bool TryGetProtocolForUuid(Guid uuid, out string protocol)
        {
            return _mapping.TryGetValue(uuid, out protocol);
        }

        public static bool TryGetUuidForProtocol(string protocol, out Guid uuid)
        {
            if (string.IsNullOrEmpty(protocol))
                throw new ArgumentNullException(nameof(protocol));

            foreach (var map in _mapping)
            {
                if (map.Value == protocol)
                {
                    uuid = map.Key;
                    return true;
                }
            }

            uuid = Guid.Empty;
            return false;
        }

        public static void Add(Guid uuid, string protocol)
        {
            _mapping[uuid] = protocol;
        }

        public static bool Remove(Guid uuid)
        {
            return _mapping.Remove(uuid);
        }
        
        public static bool Remove(string protocol)
        {
            if (string.IsNullOrEmpty(protocol))
                throw new ArgumentNullException(nameof(protocol));

            var isMatch = false;
            KeyValuePair<Guid, string> match = default;
            foreach (var map in _mapping)
            {
                if (map.Value == protocol)
                {
                    match = map;
                    isMatch = true;
                    break;
                }
            }

            return isMatch && _mapping.Remove(match);
        }
    }
}