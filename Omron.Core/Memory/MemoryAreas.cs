﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Omron.Core
{
    public class MemoryArea
    {
        //public enum PlcMemoryModes
        //{
        //    //See Section 5-2 I/O Memory Address Designations pg. 165.
        //    CSCJMode,
        //    CVMode
        //}

        //public enum MemoryAreas
        //{
        //    CIO,
        //    WR,
        //    HR,
        //    AR,
        //    TIM,
        //    CNT,
        //    DM,
        //    TK,
        //    IR,
        //    DR,
        //    ClockPulses,
        //    ConditionFlags
        //}

        //public enum VariableTypes
        //{
        //    Bit,
        //    BitWithForcedStatus,
        //    Word,
        //    WordWithForcedStatus,
        //    CompletionFlag,
        //    CompletionFlagWithForcedStatus,
        //    PV,
        //    EM,
        //    Status
        //}


        //public static VariableTypes ParseReadAddress<T>(PlcMemoryModes mode, string address, bool useHexadecimalAddressing, out MemoryAreas area, out int memoryAddress, out int? bit)
        //{
        //    //TODO: Aliases/CDM file
        //    int theMemoryAddress;
        //    int? theBit;
        //    MemoryAreas theArea;
        //    VariableTypes theVariableType;

        //    theVariableType = DetermineVariableType(typeof(T), address, useHexadecimalAddressing, out theArea, out theMemoryAddress, out theBit);

        //    if (IsVariableTypeValidForAddress(mode, theArea, theVariableType, theMemoryAddress, false))
        //    {
        //        area = theArea;
        //        memoryAddress = theMemoryAddress;
        //        bit = theBit;
        //        return theVariableType;
        //    }
        //    else
        //    {
        //        throw new InvalidOperationException("This variable type isn't valid for this address");
        //    }
        //}

        //public static VariableTypes ParseWriteAddress<T>(PlcMemoryModes mode, string address, bool useHexadecimalAddressing, out MemoryAreas area, out int memoryAddress, out int? bit)
        //{
        //    //TODO: Aliases/CDM file
        //    int theMemoryAddress;
        //    int? theBit;
        //    MemoryAreas theArea;
        //    VariableTypes theVariableType;

        //    theVariableType = DetermineVariableType(typeof(T), address, useHexadecimalAddressing, out theArea, out theMemoryAddress, out theBit);

        //    if (IsVariableTypeValidForAddress(mode, theArea, theVariableType, theMemoryAddress, true))
        //    {
        //        area = theArea;
        //        memoryAddress = theMemoryAddress;
        //        bit = theBit;
        //        return theVariableType;
        //    }
        //    else
        //    {
        //        throw new InvalidOperationException("This variable type isn't valid for this address");
        //    }
        //}

        //private static VariableTypes DetermineVariableType(Type expectedType, string address, bool isHexAddress, out MemoryAreas area, out int memoryAddress, out int? bit)
        //{
        //    Match match;
        //    Type valueType;
        //    bool isValueArray = false;

        //    area = ParseMemoryAreaFromString(address);

        //    match = AddressRegex.Match(address);

        //    if (expectedType.IsArray)
        //    {
        //        //If it's an array of values then our type is driven by the type of the array
        //        valueType = expectedType.GetElementType();
        //        isValueArray = true;
        //    }
        //    else
        //    {
        //        valueType = expectedType;
        //        isValueArray = false;
        //    }


        //    if (match.Success)
        //    {
        //        if (match.Groups.Count > 1)
        //        {
        //            //The memory address specifies as a bit, because we're addressing 4000.00 // .00 can only be made of bits 
        //            memoryAddress = Convert.ToInt32(match.Groups[0].Value, (isHexAddress) ? 10 : 16);
        //            bit = Convert.ToInt32(match.Groups[1].Value, (isHexAddress) ? 10 : 16);

        //            Contract.Assert(isValueArray = false || isValueArray && bit == 0, "If writing an array of values, the starting bit must be bit in the address must be zero.");
        //            Contract.Assert(valueType.Equals(typeof(bool)), "When using bit style addressing (e.g. 1234.01) the value being written must be a boolean value");

        //            return VariableTypes.Bit;
        //        }
        //        else
        //        {
        //            memoryAddress = Convert.ToInt32(match.Groups[0].Value, (isHexAddress) ? 10 : 16);
        //            bit = null;
        //            return VariableTypes.Word;
        //        }
        //    }

        //    throw new InvalidOperationException("Could not determine the variable type from the address");
        //}

        //private static Regex CaptureRegex = new Regex("([a-zA-Z]+)[0-9].*");
        //private static Regex AddressRegex = new Regex("[a-zA-Z]+([0-9]+)\\.?([0-9]+)?");

        //private static MemoryAreas ParseMemoryAreaFromString(string area)
        //{
        //    // if (area.)
        //    Match match = CaptureRegex.Match(area);

        //    if (match != null && match.Groups.Count > 0)
        //    {
        //        if (!string.IsNullOrEmpty(match.Groups[0].Value))
        //        {
        //            switch (match.Groups[0].Value.ToLowerInvariant())
        //            {
        //                case "cio":
        //                    return MemoryAreas.CIO;
        //                case "dm":
        //                case "d":
        //                    return MemoryAreas.DM;
        //                case "wr":
        //                case "w":
        //                    return MemoryAreas.WR;
        //                case "dr":
        //                    return MemoryAreas.DR;
        //                case "ar":
        //                case "a":
        //                    return MemoryAreas.AR;
        //                case "hr":
        //                case "h":
        //                    return MemoryAreas.HR;
        //                case "tim":
        //                case "t":
        //                    return MemoryAreas.TIM;
        //                case "cnt":
        //                case "c":
        //                    return MemoryAreas.CNT;
        //                case "ir":
        //                    return MemoryAreas.IR;
        //                default:
        //                    throw new InvalidOperationException("Invalid memory area prefix");
        //            }
        //        }
        //    }

        //    throw new InvalidOperationException("Failed to parse memory area prefix");
        //}

        //public static byte FindMemoryAreaCode(PlcMemoryModes operatingMode, MemoryAreas area, VariableTypes variableType, bool writeAccessRequired)
        //{
        //    if (operatingMode == PlcMemoryModes.CSCJMode)
        //    { 
        //        switch (variableType)
        //        {
        //            case VariableTypes.Bit:
        //                switch (area)
        //                {
        //                    case MemoryAreas.CIO:
        //                        return Convert.ToByte("30", 16);
        //                    case MemoryAreas.WR:
        //                        return Convert.ToByte("31", 16);
        //                    case MemoryAreas.HR:
        //                        return Convert.ToByte("32", 16);
        //                    case MemoryAreas.AR:
        //                        return Convert.ToByte("33", 16);
        //                    case MemoryAreas.DM:
        //                        return Convert.ToByte("02", 16);
        //                    case MemoryAreas.TK:
        //                        return Convert.ToByte("06", 16);
        //                    default:
        //                        throw new InvalidOperationException(String.Format("The memory area {0} is not valid when using {1} mode when using the memory area as {2} ", area, operatingMode, variableType));
        //                }
        //            case VariableTypes.BitWithForcedStatus:
        //                switch (area)
        //                {
        //                    case MemoryAreas.CIO:
        //                        return Convert.ToByte("70", 16);
        //                    case MemoryAreas.WR:
        //                        return Convert.ToByte("71", 16);
        //                    case MemoryAreas.HR:
        //                        return Convert.ToByte("72", 16);
        //                    default:
        //                        throw new InvalidOperationException(String.Format("The memory area {0} is not valid when using {1} mode when using the memory area as {2} ", area, operatingMode, variableType));
        //                }
        //            case VariableTypes.Word:
        //                switch (area)
        //                {
        //                    case MemoryAreas.CIO:
        //                        return Convert.ToByte("B0", 16);
        //                    case MemoryAreas.WR:
        //                        return Convert.ToByte("B1", 16);
        //                    case MemoryAreas.HR:
        //                        return Convert.ToByte("B2", 16);
        //                    case MemoryAreas.AR:
        //                        return Convert.ToByte("B3", 16);
        //                    case MemoryAreas.DM:
        //                        return Convert.ToByte("82", 16);
        //                    default:
        //                        throw new InvalidOperationException(String.Format("The memory area {0} is not valid when using {1} mode when using the memory area as {2} ", area, operatingMode, variableType));
        //                }
        //            case VariableTypes.WordWithForcedStatus:
        //                switch (area)
        //                {
        //                    case MemoryAreas.CIO:
        //                        return Convert.ToByte("F0", 16);
        //                    case MemoryAreas.WR:
        //                        return Convert.ToByte("F1", 16);
        //                    case MemoryAreas.HR:
        //                        return Convert.ToByte("F2", 16);
        //                    default:
        //                        throw new InvalidOperationException(String.Format("The memory area {0} is not valid when using {1} mode when using the memory area as {2} ", area, operatingMode, variableType));
        //                }
        //            case VariableTypes.CompletionFlag:
        //                switch (area)
        //                {
        //                    case MemoryAreas.TIM:
        //                    case MemoryAreas.CNT:
        //                        return Convert.ToByte("09", 16);
        //                    default:
        //                        throw new InvalidOperationException(String.Format("The memory area {0} is not valid when using {1} mode when using the memory area as {2} ", area, operatingMode, variableType));
        //                }
        //            case VariableTypes.Status:
        //                switch (area)
        //                {
        //                    case MemoryAreas.TK:
        //                        return Convert.ToByte("46", 16);
        //                    default:
        //                        throw new InvalidOperationException(String.Format("The memory area {0} is not valid when using {1} mode when using the memory area as {2} ", area, operatingMode, variableType));
        //                }
        //            case VariableTypes.PV:
        //                switch (area)
        //                {
        //                    case MemoryAreas.IR:
        //                        return Convert.ToByte("DC", 16);
        //                    case MemoryAreas.DR:
        //                        return Convert.ToByte("DC", 16);
        //                    case MemoryAreas.TIM:
        //                    case MemoryAreas.CNT:
        //                        return Convert.ToByte("89", 16);
        //                    default:
        //                        throw new InvalidOperationException(String.Format("The memory area {0} is not valid when using {1} mode when using the memory area as {2} ", area, operatingMode, variableType));
        //                }
        //            default:
        //                throw new InvalidOperationException(String.Format("An invalid variable type {0} was used. ", variableType));
        //        }

        //    }
        //    else
        //    {
        //        //CV Mode
        //        throw new NotImplementedException();
        //    }
        //}

        //public static bool IsVariableTypeValidForAddress(PlcMemoryModes operatingMode, MemoryAreas area, VariableTypes variableType, int memoryAddress, bool writeRequired)
        //{
        //    if (operatingMode == PlcMemoryModes.CSCJMode)
        //    {
        //        switch (area)
        //        {
        //            case MemoryAreas.CIO:

        //                switch (variableType)
        //                {
        //                    case VariableTypes.Bit:
        //                    case VariableTypes.BitWithForcedStatus:
        //                        Contract.Assert(memoryAddress.Between(0, 614315), String.Format("This memory address is out of range for {0} using the {1} data type", area, variableType));
        //                        return true;
        //                    case VariableTypes.Word:
        //                    case VariableTypes.WordWithForcedStatus:
        //                        Contract.Assert(memoryAddress.Between(0, 6143), String.Format("This memory address is out of range for {0} using the {1} data type", area, variableType));
        //                        return true;
        //                    default:
        //                        throw new InvalidOperationException("The variable type is not valid for this memory address");
        //                }
        //            case MemoryAreas.WR:
        //                switch (variableType)
        //                {
        //                    case VariableTypes.Bit:
        //                    case VariableTypes.BitWithForcedStatus:
        //                        Contract.Assert(memoryAddress.Between(0, 51115), String.Format("This memory address is out of range for {0} using the {1} data type", area, variableType));
        //                        return true;
        //                    case VariableTypes.Word:
        //                    case VariableTypes.WordWithForcedStatus:
        //                        Contract.Assert(memoryAddress.Between(0, 511), String.Format("This memory address is out of range for {0} using the {1} data type", area, variableType));
        //                        return true;
        //                    default:
        //                        throw new InvalidOperationException("The variable type is not valid for this memory address");
        //                }
        //            case MemoryAreas.DM:
        //                switch (variableType)
        //                {
        //                    case VariableTypes.Bit:
        //                        Contract.Assert(memoryAddress.Between(0, 3276715), String.Format("This memory address is out of range for {0} using the {1} data type", area, variableType));
        //                        return true;
        //                    case VariableTypes.Word:
        //                        Contract.Assert(memoryAddress.Between(0, 32767), String.Format("This memory address is out of range for {0} using the {1} data type", area, variableType));
        //                        return true;
        //                    default:
        //                        throw new InvalidOperationException("The variable type is not valid for this memory address");
        //                }
        //            case MemoryAreas.HR:
        //                switch (variableType)
        //                {
        //                    case VariableTypes.Bit:
        //                    case VariableTypes.BitWithForcedStatus:
        //                        Contract.Assert(memoryAddress.Between(0, 51115), String.Format("This memory address is out of range for {0} using the {1} data type", area, variableType));
        //                        return true;
        //                    case VariableTypes.Word:
        //                    case VariableTypes.WordWithForcedStatus:
        //                        Contract.Assert(memoryAddress.Between(0, 511), String.Format("This memory address is out of range for {0} using the {1} data type", area, variableType));
        //                        return true;
        //                    default:
        //                        throw new InvalidOperationException("The variable type is not valid for this memory address");
        //                }

        //            case MemoryAreas.AR:
        //                switch (variableType)
        //                {
        //                    case VariableTypes.Bit:
        //                    case VariableTypes.BitWithForcedStatus:
        //                        if (writeRequired)
        //                        {
        //                            Contract.Assert(memoryAddress.Between(44800, 95915), String.Format("This memory address is out of range for {0} using the {1} data type. As this is a write operation the memory area must be between: {2} and {3}", area, variableType, 44800, 95915));
        //                        }
        //                        else {
        //                            Contract.Assert(memoryAddress.Between(0, 44715), String.Format("This memory address is out of range for {0} using the {1} data type", area, variableType));
        //                        } 
        //                        return true; 
        //                    default:
        //                        throw new InvalidOperationException("The variable type is not valid for this memory address");
        //                }

        //            case MemoryAreas.TIM:
        //                switch (variableType)
        //                {
        //                    case VariableTypes.CompletionFlag:
        //                    case VariableTypes.CompletionFlagWithForcedStatus:
        //                        Contract.Assert(memoryAddress.Between(0, 4095), String.Format("This memory address is out of range for {0} using the {1} data type", area, variableType));
        //                        return true;
        //                    default:
        //                        throw new InvalidOperationException("The variable type is not valid for this memory address");
        //                }
        //            case MemoryAreas.CNT:
        //                switch (variableType)
        //                {
        //                    case VariableTypes.CompletionFlag:
        //                    case VariableTypes.CompletionFlagWithForcedStatus:
        //                        Contract.Assert(memoryAddress.Between(0, 4095), String.Format("This memory address is out of range for {0} using the {1} data type", area, variableType));
        //                        return true; 
        //                    default:
        //                        throw new InvalidOperationException("The variable type is not valid for this memory address");
        //                }

        //            default:
        //                throw new InvalidOperationException(String.Format("Invalid memory area detected {0}", area));
        //        }
                
        //    }
        //    else
        //    {
        //        throw new NotImplementedException();
        //    }
        //}

    }
}
