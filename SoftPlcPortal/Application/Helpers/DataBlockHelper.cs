using Sharp7;
using SoftPlcPortal.Infrastructure.Tables;
using static MudBlazor.CategoryTypes;

namespace SoftPlcPortal.Application.Helpers;

public class DataBlockHelper
{
    public static int GetDataTypeSize(DbDataType dataType)
    {
        return dataType switch
        {
            DbDataType.Bool => 1,
            
            DbDataType.Byte => 8,

            DbDataType.Int => 16,
            DbDataType.UInt => 16,
            DbDataType.Word => 16,

            DbDataType.DInt => 32,
            DbDataType.DWord => 32,
            DbDataType.Real => 32,

            _ => throw new ArgumentException("Datatype has no known size defined", nameof(dataType))
        };
    }
}

public static class S7ClientHelper
{
    public static void SetValue(this byte[] buffer, DbDataType dataType, int offset, object value)
    {
        switch(dataType)
        {
            case DbDataType.Bool: buffer.SetBitAt(0, offset, ConvertToBool(value)); break;
            case DbDataType.Byte: buffer.SetByteAt(offset, Convert.ToByte(value)); break;
            case DbDataType.Int: buffer.SetIntAt(offset, Convert.ToInt16(value)); break;
            case DbDataType.UInt: buffer.SetUIntAt(offset, Convert.ToUInt16(value)); break;
            case DbDataType.DInt: buffer.SetDIntAt(offset, Convert.ToInt32(value)); break;
            case DbDataType.Real: buffer.SetRealAt(offset, Convert.ToSingle(value)); break;
            case DbDataType.Word: buffer.SetWordAt(offset, Convert.ToUInt16(value)); break;
            case DbDataType.DWord: buffer.SetDWordAt(offset, Convert.ToUInt32(value)); break;
        }
    }

    private static bool ConvertToBool(object value)
    {
        if (value is bool b)
            return b;

        if (decimal.TryParse(value.ToString(), out var d))
            return d > 0;

        return false;
    }
}
