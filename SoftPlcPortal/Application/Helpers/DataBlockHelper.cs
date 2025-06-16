using DocumentFormat.OpenXml.Drawing;
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

            DbDataType.DInt => 32,
            //DbDataType.String => 32,
            DbDataType.Real => 32,

            DbDataType.LReal => 64,
            DbDataType.LInt => 64,
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

            DbDataType.DInt => 4,
            //DbDataType.String => 4,
            DbDataType.Real => 4,

            DbDataType.LReal => 8,
            DbDataType.LInt => 8,
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
            case DbDataType.Bool:
                buffer.SetBitAt(0, offset, ConvertToBool(value));
                break;

            case DbDataType.Byte:
                buffer.SetByteAt(offset, value is byte b ? b : Convert.ToByte(value));
                break;

            case DbDataType.Int:
                buffer.SetIntAt(offset, value is short s ? s : Convert.ToInt16(value));
                break;

            case DbDataType.DInt:
                buffer.SetDIntAt(offset, value is int i ? i : Convert.ToInt32(value));
                break;

            case DbDataType.LInt:
                buffer.SetLIntAt(offset, value is long l ? l : Convert.ToInt64(value));
                break;

            case DbDataType.Real:
                buffer.SetRealAt(offset, value is float f ? f : Convert.ToSingle(value));
                break;

            case DbDataType.LReal:
                buffer.SetLRealAt(offset, value is double d ? d : Convert.ToDouble(value));
                break;

                //case DbDataType.String:
                //    buffer.SetStringAt(offset, MaxLen: buffer.Length, value is string str ? str : Convert.ToString(value));
                //    break;
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
            DbDataType.DInt => data.GetDIntAt(dbField.ByteOffset).ToString(),
            DbDataType.LInt => data.GetLIntAt(dbField.ByteOffset).ToString(),
            DbDataType.Real => data.GetRealAt(dbField.ByteOffset).ToString(),
            DbDataType.LReal => data.GetLRealAt(dbField.ByteOffset).ToString(),
            //DbDataType.String => data.GetStringAt(dbField.ByteOffset).ToString(),
            _ => throw new NotSupportedException("Unsupported db field data type")
        };
    }
}
