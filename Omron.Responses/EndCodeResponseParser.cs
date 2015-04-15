using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Omron.Responses
{
    public class EndCodeResponseParser
    {

        private static string GetSubcodeDescription(int mainCode, int subCode)
        {
            switch (mainCode)
            {
                case 1:
                    switch (subCode)
                    {
                        case 1:
                            return "Local Node not in network";
                        case 2:
                            return "Token timeout";
                        case 3:
                            return "Retries failed";
                        case 4:
                            return "Too many send frames";
                        case 5:
                            return "Node address range error";
                        case 6:
                            return "Node address duplication";
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
            return null;
        }

        private static string GetSubcodeCheckpoint(int mainCode, int subCode)
        {
            switch (mainCode)
            {
                case 1:
                    switch (subCode)
                    {
                        case 1:
                            return "Network status of the local node";
                        case 2:
                            return "Maximum node address";
                        case 3:
                            return "--";
                        case 4:
                            return "Number of enabled send frames";
                        case 5:
                            return "Node address";
                        case 6:
                            return "Node addresses";
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
            return null;
        }

        public class LocalNodeErrorException : Exception
        {
            public int SubCode { get; set; }
            public string SubCodeDescription
            {
                get
                {
                    return GetSubcodeDescription(1, this.SubCode);
                }
            }
            public LocalNodeErrorException(int subcode, int mainCode)
            {
                this.SubCode = subcode;
                this.MainCode = mainCode;
            }

            public override string ToString()
            {
                var sb = new StringBuilder();

                sb.AppendFormat("{0}: \"{1}\" (Main: {2}, Sub-code: {3}) Please check {4}", this.GetType().Name, this.SubCodeDescription, 1, this.SubCode, GetSubcodeCheckpoint(this.MainCode, this.SubCode));
                sb.AppendLine().Append(base.ToString());

                return sb.ToString();
            }

            public int MainCode { get; set; }
        }

        public static bool IsValidEndCode(byte mainCode, byte subCode)
        {
            return mainCode == 0 && subCode == 0;
        }
    }
}
