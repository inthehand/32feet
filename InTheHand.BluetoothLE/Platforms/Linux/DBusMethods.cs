using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Linux.Bluetooth;
using Tmds.DBus;

namespace InTheHand.Bluetooth
{
    internal static class DBusMethods
    {
        /// <param name="interfaceName">The interface to search for</param>
        /// <param name="rootObject">The DBus object to search under. Can be null</param>
        public static async Task<IReadOnlyList<T>> GetProxiesAsync<T>(string interfaceName, IDBusObject? rootObject = null)
        {
            // Console.WriteLine("GetProxiesAsync called.");
            var objectManager = Connection.System.CreateProxy<IObjectManager>(BluezConstants.DbusService, "/");
            var objects = await objectManager.GetManagedObjectsAsync();

            var matchingObjectPaths = objects
                .Where(obj => IsMatch(interfaceName, obj.Key, obj.Value, rootObject))
                .Select(obj => obj.Key);

            var proxies = matchingObjectPaths
                .Select(objectPath => Connection.System.CreateProxy<T>(BluezConstants.DbusService, objectPath))
                .ToList();

            // Console.WriteLine($"GetProxiesAsync returning {proxies.Count} proxies of type {typeof(T)}.");
            return proxies;
        }

        private static bool IsMatch(string interfaceName, ObjectPath objectPath, IDictionary<string, IDictionary<string, object>> interfaces, IDBusObject? rootObject)
        {
            return IsMatch(interfaceName, objectPath, interfaces.Keys, rootObject);
        }

        private static bool IsMatch(string interfaceName, ObjectPath objectPath, ICollection<string> interfaces, IDBusObject? rootObject)
        {
            if (rootObject != null && !objectPath.ToString().StartsWith($"{rootObject.ObjectPath}/"))
            {
                return false;
            }

            return interfaces.Contains(interfaceName);
        }
    }
}