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
                if (value is byte b)
                    buffer.SetByteAt(offset, b);
                else
                    throw new ArgumentException("Value must be of type byte for DbDataType.Byte", nameof(value));
                break;

            case DbDataType.Int:
                if (value is short s)
                    buffer.SetIntAt(offset, s);
                else
                    throw new ArgumentException("Value must be of type short for DbDataType.Int", nameof(value));
                break;

            case DbDataType.DInt:
                if (value is int i)
                    buffer.SetDIntAt(offset, i);
                else
                    throw new ArgumentException("Value must be of type int for DbDataType.DInt", nameof(value));
                break;

            case DbDataType.LInt:
                if (value is long l)
                    buffer.SetLIntAt(offset, l);
                else
                    throw new ArgumentException("Value must be of type long for DbDataType.LInt", nameof(value));
                break;

            case DbDataType.Real:
                if (value is float f)
                    buffer.SetRealAt(offset, f);
                else
                    throw new ArgumentException("Value must be of type float for DbDataType.Real", nameof(value));
                break;

            case DbDataType.LReal:
                if (value is double d)
                    buffer.SetLRealAt(offset, d);
                else
                    throw new ArgumentException("Value must be of type double for DbDataType.LReal", nameof(value));
                break;

                //case DbDataType.String:
                //    if (value is string str)
                //        buffer.SetStringAt(offset, MaxLen: buffer.Length, str);
                //    else
                //        throw new ArgumentException("Value must be of type string for DbDataType.String", nameof(value));
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
