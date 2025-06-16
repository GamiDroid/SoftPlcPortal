using System.ComponentModel.DataAnnotations;

namespace SoftPlcPortal.Infrastructure.Tables;

public record DbField
{
    [Key] public Guid Key { get; set; }
    public Guid DataBlockKey { get; set; }
    public int ByteOffset { get; set; }
    public int BitOffset { get; set; }
    public DbDataType DataType { get; set; }
    public required string Name { get; set; }
    public string? Comment { get; set; }
    public string? StartValue { get; set; }

    public DataBlock? DataBlock { get; set; }
}

public enum DbDataType
{
    Bool,
    Byte,
    Int,
    DInt,
    LInt,
    Real,
    LReal,
    //String,
}
