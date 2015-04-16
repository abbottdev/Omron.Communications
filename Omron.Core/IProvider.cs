using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omron.Core
{
    public interface IDriver : IDisposable
    {

        /// <summary>
        /// Connects to the configured plc.
        /// </summary>
        /// <returns></returns>
        Task<bool> ConnectAsync();

        /// <summary>
        /// Disconnects from the remote Plc
        /// </summary>
        void Disconnect();

        /// <summary>
        /// Reads from the specified address of the Plc with a specified number of reads.
        /// </summary>
        /// <param name="address">The memory address to read data from. (E.g. D4000.01, or CIO2000) The return type is inferred from the address, if the read command references a bit address, the read will return bits instead of words.</param>
        /// <param name="readLength">The number of reads to perform on the interred read type.</param>
        /// <returns></returns>
        Task<object> ReadAreaAsync(string address, int readLength, byte plcUnit = 0);

        /// <summary>
        /// Writes the data to the specified area
        /// </summary>
        /// <param name="area"></param>
        /// <param name="dataToWrite"></param>
        /// <returns></returns>
        Task WriteAreaAsync(string address, byte[] data, byte plcUnit = 0);

        /// <summary>
        /// Reads the current cycle time from the Plc asynchronously.
        /// </summary>
        /// <returns></returns>
        Task<float> ReadCycleTimeAsync();

        /// <summary>
        /// Returns if there is a connection to the target plc.
        /// </summary>
        bool Connected { get; }

    }
}
