using BreakInfinity;
using Mirror;
using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BigDoubleReaderWriter
{
    public static void WriteBigDouble(this NetworkWriter writer, BigDouble bigDouble)
    {
        writer.WriteDouble(bigDouble.Mantissa);
        writer.WriteLong(bigDouble.Exponent);
    }

    public static BigDouble ReadBigDouble(this NetworkReader reader)
    {
        return new BigDouble(reader.ReadDouble(), reader.ReadLong());
    }
}
