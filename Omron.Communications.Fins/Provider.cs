using Ninject;
using Omron.Commands;
using Omron.Commands.Builder.Expressions;
using Omron.Commands.Builder.Generators; 
using Omron.Communications.Windows.Tcp;
using Omron.Responses;
using Omron.Responses.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omron.Communications.Windows
{
    /// <summary>
    /// Main class used to communicate with devices.
    /// </summary>
    public class Provider : DeviceBase
    {

        /// <summary>
        /// A helper method to help generate the frame used by an Expression.
        /// </summary>
        /// <typeparam name="TExpression"></typeparam>
        /// <typeparam name="TCommand"></typeparam>
        /// <param name="builder"></param>
        /// <param name="provider"></param>
        /// <param name="device"></param>
        /// <returns></returns>
        private Frames.Frame BuildFrameForExpression<TExpression, TCommand>(TExpression builder, ICommuncationProvider provider, PlcConfiguration device)
            where TExpression : ICommandExpression<TCommand>
            where TCommand : ICommand
        {
            Frames.Frame frameToSend;
            ICommandGenerator<TExpression, TCommand> generator;

            //Ensure that TExpression is registered/bound via ioc to a relevant generator.
            generator = this.Kernel.Get<ICommandGenerator<TExpression, TCommand>>();

            if (generator == null)
                throw new ArgumentNullException("Failed to locate a generator for expression type {0}", typeof(TExpression).Name);

            //Build the frame.
            frameToSend = generator.Generate(builder);

            return frameToSend;
        }

        /// <summary>
        /// Ensures this provider has an active connection to the Plc device, re-connecting if required.
        /// </summary>
        /// <param name="provider">The <see cref="ICommunicationProvider">provider</see> to use for the connection.</param>
        /// <param name="device">The <see cref="PlcConfiguration">configuration</see> for the device to connect to.</param>
        /// <returns></returns>
        private static async Task EnsureConnectionAsync(ICommuncationProvider provider, PlcConfiguration device)
        {
            if (!provider.Connected)
            {
                if (!await provider.ConnectAsync(device))
                {
                    throw new InvalidOperationException("Communication Provider could not connect to device");
                }
            }
        }

        public Provider(PlcConfiguration configuration)
            : base(configuration, new StandardKernel())
        {

        }

        /// <summary>
        /// Sends a built <see cref="Frames.Frame">Frame</see> to the configured device via the passed provider.
        /// </summary>
        /// <param name="frame">The <see cref="Frames.Frame">Frame</see> to send</param>
        /// <param name="provider">The <see cref="ICommuncationProvider">communication provider</see> to use for the connection.</param>
        /// <param name="device">The <see cref="PlcConfiguration">configuration</see> used to connect to the device</param>
        /// <returns></returns>
        public static async Task DispatchFrameAsync(Frames.Frame frame, ICommuncationProvider provider, PlcConfiguration device)
        {

            await EnsureConnectionAsync(provider, device);

            //Send the frame to the device via the provider
            await provider.SendAsync(frame);
        }

        /// <summary>
        /// Asynchronously reads from a memory area 
        /// </summary>
        /// <param name="area"></param>
        /// <param name="readLength"></param>
        /// <returns></returns>
        public override async Task<string> ReadAreaAsync(string area, int readLength)
        {
            Frames.Frame receivedFrame, frameToSend; 
             IResponseForReadCommand response;

            //Ensure that TExpression is registered/bound via ioc to a relevant generator.
            frameToSend = BuildFrameForExpression<IReadCommandExpression, IReadCommand>(ReadAreaCommandBuilder.ForArea(area), Provider, Configuration);

            //Send the frame.
            await DispatchFrameAsync(frameToSend, Provider, Configuration);

            //Recieve the response.
            receivedFrame = await Provider.ReceiveAsync();

            //Parse the Fins Response Frame.  
            //Try parse the response out from the frame.
            response = Parser.ParseResponse<IResponseForReadCommand, Commands.IReadCommand>(receivedFrame);

            if (response == null)
                throw new ArgumentException("Response command was correct, but type was unexpected");

            //Ensure the response is one that we want for a read command.
            System.Diagnostics.Contracts.Contract.Assert(response.CommandType == Commands.CommandTypes.Read);

            return response.Response;
        }

        /// <summary>
        /// Registered and binds the types used for this provider.
        /// </summary>
        /// <param name="configuration"></param>
        public override void RegisterAndBindTypes(PlcConfiguration configuration)
        {
            //Communication Provider 
            Kernel
                .Bind<ICommuncationProvider>()
                .To<Omron.Communications.Windows.Tcp.TcpCommunicationProvider>()
                .When(request => configuration.Serial == false)
                .InThreadScope();

            //Use Serial port provider if configuration is set to use it.
            Kernel
                .Bind<ICommuncationProvider>()
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
        private static void  BindTypesForSerialCommunication(PlcConfiguration configuration, IKernel kernel)
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
              .When(request => kernel.Get<ICommuncationProvider>().GetType() == typeof(TcpCommunicationProvider));

            //Commands - Read Command
            kernel
                .Bind<IReadCommandExpression>()
                .To<Commands.Expressions.Fins.ReadCommandExpression>()
                .When(request => kernel.Get<ICommuncationProvider>().GetType() == typeof(TcpCommunicationProvider));

            //Commands - Write Command
            //TODO:

            //Command Generators - ReadCommandExpression. Used to generate the command frame that's sent to the plc.
            kernel
                .Bind<ICommandGenerator<IReadCommandExpression, IReadCommand>>()
                .To<Omron.Commands.Expressions.Generators.Fins.ReadCommandGenerator>()
                .When(request => kernel.Get<ICommuncationProvider>().GetType() == typeof(TcpCommunicationProvider));

        }

        /// <summary>
        /// Writes the data to the specified area
        /// </summary>
        /// <param name="area"></param>
        /// <param name="dataToWrite"></param>
        /// <returns></returns>
        public override Task WriteAreaAsync(string area, byte[] dataToWrite)
        {
            throw new NotImplementedException();
        }
    }
}
