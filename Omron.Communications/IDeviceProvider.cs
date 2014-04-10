using Ninject;
using Omron.Commands.Builder.Expressions;
using Omron.Core;
using Omron.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omron.Communications
{
    public interface IDevice
    {
        Task<string> ReadAreaAsync(string area, int readLength);
        Task WriteAreaAsync(string area, byte[] dataToWrite);
    }

    public abstract class DeviceBase : IDevice
    {
        protected IKernel Kernel { get; private set; }
        protected PlcConfiguration Configuration { get; private set; }
        protected ICommuncationProvider Provider { get; private set; }
        protected IResponseParser Parser { get; private set; }

        protected IReadCommandExpression ReadAreaCommandBuilder { get; private set; } 
       
        public DeviceBase(PlcConfiguration configuration, IKernel kernel)
        {
            this.Kernel = kernel;
            this.Configuration = configuration;

            this.RegisterAndBindTypes(configuration);

            this.Provider = kernel.Get<ICommuncationProvider>();
            this.Parser = kernel.Get<IResponseParser>();
            this.ReadAreaCommandBuilder = kernel.Get<IReadCommandExpression>();
        }

        public abstract void RegisterAndBindTypes(PlcConfiguration configuration);
        public abstract Task<string> ReadAreaAsync(string area, int readLength);
        public abstract Task WriteAreaAsync(string area, byte[] dataToWrite);
    }
}
