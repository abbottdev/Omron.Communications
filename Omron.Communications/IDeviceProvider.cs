﻿using Ninject;
using Omron.Commands;
using Omron.Commands.Expressions;
using Omron.Commands.Generators;
using Omron.Core;
using Omron.Core.Frames;
using Omron.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omron.Communications
{

    public abstract class ProviderBase : IDisposable
    {
        protected IKernel Kernel { get; private set; }
        protected PlcConfiguration Configuration { get; private set; }
        protected IConnection Provider { get; private set; }
        protected IResponseParser Parser { get; private set; }

        protected IReadCommandExpression ReadAreaCommandBuilder { get; private set; }

        public ProviderBase(PlcConfiguration configuration, IKernel kernel)
        {
            this.Kernel = kernel;
            this.Configuration = configuration;

            this.RegisterAndBindTypes(configuration);

            this.Provider = kernel.Get<IConnection>();
            this.Parser = kernel.Get<IResponseParser>();
            this.ReadAreaCommandBuilder = kernel.Get<IReadCommandExpression>();
        }
        
        /// <summary>
        /// Asynchronously reads from a memory area 
        /// </summary>
        /// <param name="area"></param>
        /// <param name="readLength"></param>
        /// <returns></returns>
        protected async Task<string> ReadAreaAsync(string area, int readLength)
        {
            Core.Frames.Frame receivedFrame, frameToSend;
            IResponseForReadCommand response;

            //Validate the connection to the host
            VerifyConnection();

            //Ensure that TExpression is registered/bound via ioc to a relevant generator.
            frameToSend = BuildFrameForCommand<IReadCommand>(ReadAreaCommandBuilder.ForArea(area).WithNumberOfItems(readLength).GetCommand(), Provider, Configuration);

            //Send the frame to the device via the provider
            await Provider.SendAsync(frameToSend);

            //Recieve the response.
            receivedFrame = await Provider.ReceiveAsync();

            //Parse the Fins Response Frame.  
            //Try parse the response out from the frame.
            response = Parser.ParseResponse<IResponseForReadCommand, IReadCommand>(receivedFrame);
             
            if (response == null)
                throw new ArgumentException("Response command was correct, but type was unexpected");

            return response.Response;
        }

        protected async Task WriteAreaAsync(string area, byte[] dataToWrite)
        {
            throw new NotImplementedException();
        }



        /// <summary>
        /// A helper method to help generate the frame used by an Expression.
        /// </summary>
        /// <typeparam name="TExpression"></typeparam>
        /// <typeparam name="TCommand"></typeparam>
        /// <param name="builder"></param>
        /// <param name="provider"></param>
        /// <param name="device"></param>
        /// <returns></returns>
        protected Frame BuildFrameForCommand<TCommand>(TCommand command, IConnection provider, PlcConfiguration device)
            where TCommand : ICommand
        {
            Frame frameToSend;
            IFrameGeneratorOf<TCommand> generator;

            //Ensure that TExpression is registered/bound via ioc to a relevant generator.
            generator = this.Kernel.Get<IFrameGeneratorOf<TCommand>>();

            if (generator == null)
                throw new ArgumentNullException("Failed to locate a generator for expression type {0}", typeof(TCommand).Name);

            //Build the frame.
            frameToSend = generator.Generate(command, device, provider);

            return frameToSend;
        }

        private void VerifyConnection()
        {
            if (Provider == null || Provider.Connected == false)
            {
                throw new InvalidOperationException("You must connect to the device using the ConnectAsync method before calling any other operation");
            }
        }

        ///// <summary>
        ///// Ensures this provider has an active connection to the Plc device, re-connecting if required.
        ///// </summary>
        ///// <param name="provider">The <see cref="IConnection">provider</see> to use for the connection.</param>
        ///// <param name="device">The <see cref="PlcConfiguration">configuration</see> for the device to connect to.</param>
        ///// <returns></returns>
        //protected async Task EnsureConnectionAsync()
        //{
        //    if (!Provider.Connected)
        //    {
        //        if (!await Provider.ConnectAsync(Configuration))
        //        {
        //            throw new InvalidOperationException("Communication Provider could not connect to device");
        //        } 
        //    }
        //}


        public abstract void RegisterAndBindTypes(PlcConfiguration configuration);

        public void Dispose()
        {
            if (Provider != null)
            {
                if (Provider.Connected)
                    Provider.Disconnect();
            }
        }
    }
}
