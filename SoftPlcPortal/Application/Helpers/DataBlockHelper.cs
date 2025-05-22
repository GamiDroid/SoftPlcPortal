using Sharp7;
using SoftPlcPortal.Infrastructure.Tables;

namespace SoftPlcPortal.Application.Helpers;

public class DataBlockHelper
{
    public static int GetDataTypeBitSize(DbDataType dataType)
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

    public static int GetDataTypeMinimumByteSize(DbDataType dataType)
    {
        return dataType switch
        {
            DbDataType.Bool => 1,

            DbDataType.Byte => 1,

            DbDataType.Int => 2,
            DbDataType.UInt => 2,
            DbDataType.Word => 2,

            DbDataType.DInt => 4,
            DbDataType.DWord => 4,
            DbDataType.Real => 4,

            _ => throw new ArgumentException("Datatype has no known size defined", nameof(dataType))
        };
    }
}

public static class S7ClientHelper
{
    public static void SetValue(this byte[] buffer, DbDataType dataType, int offset, object value)
    {
        switch (dataType)
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

    public static string GetValueAsText(this DbField dbField, byte[] data)
    {
        if (data.Length < dbField.ByteOffset + 1 + DataBlockHelper.GetDataTypeMinimumByteSize(dbField.DataType) || dbField.ByteOffset < 0)
            return "<OUT_OF_RANGE>";

        return dbField.DataType switch
        {
            DbDataType.Bool => data.GetBitAt(dbField.ByteOffset, dbField.BitOffset).ToString(),
            DbDataType.Byte => data.GetByteAt(dbField.ByteOffset).ToString(),
            DbDataType.Int => data.GetIntAt(dbField.ByteOffset).ToString(),
            DbDataType.UInt => data.GetUIntAt(dbField.ByteOffset).ToString(),
            DbDataType.DInt => data.GetDIntAt(dbField.ByteOffset).ToString(),
            DbDataType.Real => data.GetRealAt(dbField.ByteOffset).ToString(),
            DbDataType.Word => data.GetWordAt(dbField.ByteOffset).ToString(),
            DbDataType.DWord => data.GetDWordAt(dbField.ByteOffset).ToString(),
            _ => throw new NotSupportedException("Unsupported db field data type")
        };
    }
}
