using Ninject;
using Omron.Commands;
using Omron.Commands.Expressions;
using Omron.Commands.Generators;
using Omron.Communications.Windows.Tcp;
using Omron.Core;
using Omron.Responses;
using Omron.Responses.Implementation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omron.Communications
{
    /// <summary>
    /// Main class used to communicate with devices.
    /// </summary>
    public class Provider : ProviderBase, IProvider
    {

        public Provider(PlcConfiguration configuration)
            : base(configuration, new StandardKernel())
        {
        }

        /// <summary>
        /// Registered and binds the types used for this provider.
        /// </summary>
        /// <param name="configuration"></param>
        public override void RegisterAndBindTypes(PlcConfiguration configuration)
        {
            //Communication Provider 
            Kernel
                .Bind<IConnection>()
                .To<Omron.Communications.Windows.Tcp.TcpCommunicationProvider>()
                .When(request => configuration.Serial == false)
                .InThreadScope();

            //Use Serial port provider if configuration is set to use it.
            Kernel
                .Bind<IConnection>()
                .To<Omron.Communications.Windows.SerialPort.SerialPortCommunicationProvider>()
                .When(request => configuration.Serial == true)
                .InThreadScope();


            BindTypesForTcpCommunication(configuration, base.Kernel);

            BindTypesForSerialCommunication(configuration, base.Kernel);

        }

        /// <summary>
        /// Binds the types that should be used under serial communication
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="kernel"></param>
        private static void BindTypesForSerialCommunication(PlcConfiguration configuration, IKernel kernel)
        {

        }

        /// <summary>
        /// Binds the types that should be used under Tcp communication
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="kernel"></param>
        private static void BindTypesForTcpCommunication(PlcConfiguration configuration, IKernel kernel)
        {
            //Response Parser 
            kernel
              .Bind<IResponseParser>()
              .To<Responses.Fins.FinsResponseParser>()
                .When(request => kernel.Get<IConnection>().ProtocolType == ProtocolTypes.FinsTcpIp);

            //Commands - Read Command
            kernel
                .Bind<IReadCommandExpression>()
                .To<Commands.Expressions.ReadCommandExpression>();

            //Commands - Write Command
            //TODO:

            //Command Generators - ReadCommandExpression. Used to generate the command frame that's sent to the plc.
            kernel
                .Bind<IFrameGeneratorOf<IReadCommand>>()
                .To<Omron.Commands.Generators.Fins.ReadCommandFrameGenerator>()
                .When(request => kernel.Get<IConnection>().ProtocolType == ProtocolTypes.FinsTcpIp);

            kernel
                .Bind<IFrameGeneratorOf<IConnectionCommand>>()
                .To<Omron.Commands.Generators.Fins.ConnectionFrameGenerator>()
                .When(request => kernel.Get<IConnection>().ProtocolType == ProtocolTypes.FinsTcpIp);
        }

        /// <summary>
        /// Connects to the configured plc.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> ConnectAsync()
        {
            ConnectionCommand command;
            Core.Frames.Frame frame, receivedFrame;

            if (!base.Provider.Connected)
            {
                if (await base.Provider.ConnectAsync(base.Configuration))
                {

                    //Generate an initial connection command if one is required for this protocol/transport.
                    command = new ConnectionCommand();

                    frame = Kernel.Get<IFrameGeneratorOf<IConnectionCommand>>().Generate(command, Configuration, Provider);

                    await Provider.SendAsync(frame);

                    receivedFrame = await Provider.ReceiveAsync();

                    //TODO: Does the received frame need any further parsing?
                    //For TcpIp/Fins I don't believe it's required as the SourceunitAddress (SA1) is auto allocated
                    //But for other protocols such as Udp, HostLink etc, this may be required and be persisted somewhere. 
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Disconnects from the remote Plc
        /// </summary>
        public void Disconnect()
        {
            if (Provider != null)
                Provider.Disconnect();
        }

        /// <summary>
        /// Writes the data to the specified area
        /// </summary>
        /// <param name="area"></param>
        /// <param name="dataToWrite"></param>
        /// <returns></returns>
        public new async Task WriteAreaAsync(string address, byte[] data)
        {
            await base.WriteAreaAsync(address, data);
        }

        /// <summary>
        /// Reads the current cycle time from the Plc asynchronously.
        /// </summary>
        /// <returns></returns>
        public async Task<float> ReadCycleTimeAsync()
        {
            throw new NotImplementedException();
        }


        public new async Task<string> ReadAreaAsync(string address, int readLength)
        {
            return await base.ReadAreaAsync(address, readLength);
        }


        public bool Connected
        {
            get
            {
                return (Provider == null) ? false : Provider.Connected;
            }
        }
    }
}
