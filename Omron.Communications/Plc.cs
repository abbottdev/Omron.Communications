using Ninject;
using Omron.Core;
using System;
using System.Collections.Generic; 
using System.Reactive.Linq;
using react = System.Reactive;
using System.Text;
using System.Linq;
using System.Reactive.Threading;

namespace Omron
{
    public class MemoryAreaValue<T>
    {
        private string address;
        private T value;
       public string Address { get { return this.address; } }
       public T Value { get { return this.value; } }

        public MemoryAreaValue(string address, T value)
        {
            this.address = address;
            this.value = value;
        }
    }

    public class Plc
    {
        private IKernel kernel;
        private IDriver driver;

        public Plc(IKernel kernel, IDriver driver)
        {
            this.kernel = kernel;
            this.driver = driver;
        }

        private static void AddOrUpdate(Dictionary<string, int> dictionary, string key, int value)
        {
            if (dictionary.ContainsKey(key)) {
                dictionary[key] = value;
            }
            else
            {
                dictionary.Add(key, value);
            }
        }

        private IObservable<MemoryAreaValue<int>> ObserveArea(string startAddress, string endAddress, TimeSpan frequency)
        {
            Dictionary<string, int> values = new Dictionary<string, int>();
            IEnumerable<string> addressesToMonitor = this.CreateEnumerableForAddressRange(startAddress, endAddress);
            Dictionary<string, int> addressesSplitToRangesWithNumberOfReads;
            object gate = new object();

            addressesSplitToRangesWithNumberOfReads = this.SplitAddresses(addressesToMonitor, 995);

            return Observable
                .Interval(frequency)
                .SelectMany<long, MemoryAreaValue<int>>(new Func<long, IEnumerable<MemoryAreaValue<int>>>((l) =>
                {
                    //Split addresses to monitor into ranges of 999 because that's the max we can read from any PLC area.
                    return addressesSplitToRangesWithNumberOfReads.SelectMany<KeyValuePair<string, int>, MemoryAreaValue<int>>((kvp) =>
                    {
                        string start = kvp.Key;
                        int numberOfReads = kvp.Value;

                        int[] result = (int[])this.driver.ReadAreaAsync(start, numberOfReads).Result;

                        var r = result.Select((index, value) => new MemoryAreaValue<int>((Convert.ToInt32(start) + index).ToString(), value));

                        return r;
                    });
                }))
                .Synchronize(gate)
                .Where(reading =>
                {
                    if (values.ContainsKey(reading.Address) == false || values[reading.Address] != reading.Value)
                    {
                        AddOrUpdate(values, reading.Address, reading.Value);
                        return true;
                    }
                    else { return false; }
                });
                
        }

        private Dictionary<string, int> SplitAddresses(IEnumerable<string> addressesToMonitor, int p)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<string> CreateEnumerableForAddressRange(string startAddress, string endAddress)
        {
            throw new NotImplementedException();
        }

    }
}
