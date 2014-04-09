using Omron.Frames;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Omron.Commands.Frames.Fins
{

    public partial class FinsCommandFrame : Frame, ICommandFrame
    {

        public class FinsCommandHeader
        {
            private FinsCommandFrame command;

            public FinsCommandHeader(FinsCommandFrame frame)
            {
                command = frame;
                InitialiseHeader();
            }

            public bool ResponseRequired
            {
                get
                {
                    return command.GetBit((int)FinsHeaderFields.ICF, 0) == false;
                }
                set
                {
                    command.SetBit((int)FinsHeaderFields.ICF, 0, !value);
                }
            }

            private void InitialiseHeader()
            {
                command.SetByte((int)FinsHeaderFields.ICF, (byte)0);
                command.SetBit((int)FinsHeaderFields.ICF, 7, true);

                //GCT - Permissable no of gateways - set to 02 (Hex)
                command.SetByteHex((int)FinsHeaderFields.GCT, "03");

                //DNA - Destionation network address
                command.SetByte((int)FinsHeaderFields.DNA, 0);


                //DA1 - Destination node Address
                //Speifices the number of the node where the command is being sent. This node address is the address used for FINS and is different from the IP adddress used for ethernet.
                DestinationNodeAddress = 0; //00 Hex - Local PLC Unit, 01-FE - Destination Node address (1 -254), FF(Hex) - Broadcasting.

                //DA2 - Destination unit Address 0- PLC (CpuUnit)
                DestinationUnitAddress = 0;

                //SNA - Source Network address.

                ServiceId = ServiceManager.GetServiceId();

                ResponseRequired = true;
            }

            /// <summary>
            /// DA1 - Destination Node Address. Specifies the number of the node where the command is being sent. This node address is the address used for FINS and is different from the IP adddress used for ethernet
            /// </summary>
            public byte DestinationNodeAddress
            {
                get
                {
                    return command.GetByte((int)FinsHeaderFields.DA1);
                }
                set
                {
                    command.SetByte((int)FinsHeaderFields.DA1, value);
                }
            }

            /// <summary>
            /// DA2 - Destination Unit Address. Specifies the number of the unit at the destination node. 00 (Hex): PLC (Cpu Unit), 10-1F (Hex): CPU Bus unit numbers - to 15 (16 to 31), 
            /// E1 (Hex): Inner Boad, FE (Hex): Unit connected to network.
            /// </summary>
            public byte DestinationUnitAddress
            {
                get
                {
                    return command.GetByte((int)FinsHeaderFields.DA1);
                }
                set
                {
                    command.SetByte((int)FinsHeaderFields.DA1, value);
                }
            }

            /// <summary>
            /// SNA - Source Network Address. Specifies the number of the network where the local node is located. The ranges of numbers that can be specified are the same as for DNA.
            /// </summary>
            public byte SourceNetworkAddress
            {
                get
                {
                    return command.GetByte((int)FinsHeaderFields.SNA);
                }
                set
                {
                    command.SetByte((int)FinsHeaderFields.SNA, value);
                }
            }

            /// <summary>
            /// SA1 - Source Node Address, specifies the local node address. The ranges of numbers that can be specified are the same as for DA1.
            /// </summary>
            public byte SourceNodeAddress
            {
                get
                {
                    return command.GetByte((int)FinsHeaderFields.SA1);
                }
                set
                {
                    command.SetByte((int)FinsHeaderFields.SA1, value);
                }
            }

            /// <summary>
            /// SA2 - Source Unit Address, specifies the number of the Unit at the local node. The ranges of numbers that can be specified are the same as for DA2.
            /// </summary>
            public byte SourceUnitAddress
            {
                get
                {
                    return command.GetByte((int)FinsHeaderFields.SA2);
                }
                set
                {
                    command.SetByte((int)FinsHeaderFields.SA2, value);
                }
            }

            /// <summary>
            /// SID - Service ID, The SID is used to identify the process that data is sent from. Any number can be set 
            /// between 00 to FF hexadecimal for the SID. The same value set for the SID in the command is returned by 
            /// the node sending the response, allowing commands and responses to be matched when commands are sent in 
            /// succession to the same Unit.
            /// </summary>
            public byte ServiceId
            {
                get
                {
                    return command.GetByte((int)FinsHeaderFields.SID);
                }
                set
                {
                    command.SetByte((int)FinsHeaderFields.SID, value);
                }
            }

        }

    }
}
