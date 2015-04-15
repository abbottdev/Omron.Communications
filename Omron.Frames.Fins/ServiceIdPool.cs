using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Omron.Commands.Frames.Fins
{
    public sealed class ServiceManager
    {
        private static int serviceIdLast; 

        public static byte GetServiceId()
        {
            var serviceId = Interlocked.Increment(ref serviceIdLast);
            
            //Reset to zero if at max value
            Interlocked.CompareExchange(ref serviceIdLast, (int)byte.MinValue, (int)byte.MaxValue);

            return (byte)serviceId;
        }

        //private static Core.ResourcePool<byte> _instance;
        //public static Core.ResourcePool<byte> Pool { get { return _instance; } }

        //public enum ReservedServices
        //{
        //    Reserved = 1
        //}


        //static ServiceIdPool()
        //{
        //    List<byte> pooledServiceIds;
        //    int maxmiumReservedServiceId = 1;
        //    IEnumerable<ReservedServices> values;

        //    values = (from v in Enum.GetValues(typeof(ReservedServices)).Cast<ReservedServices>() select v);

        //    //Get the max no of reserved services from the enum
        //    maxmiumReservedServiceId = values.Max(r => (int)r);

        //    //Initialise the pool of bytes for service Id.
        //    pooledServiceIds = new List<byte>();

        //    for (int i = maxmiumReservedServiceId; i < Byte.MaxValue; i++)
        //    {
        //        pooledServiceIds.Add((byte)i);
        //    }

        //    _instance = new Core.ResourcePool<byte>(Byte.MaxValue - 1, pooledServiceIds);
        //}

    }
}
