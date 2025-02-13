using System.ComponentModel.DataAnnotations;

namespace SoftPlcPortal.Infrastructure.Tables;

public record PlcConfig
{
    [Key] public Guid Key { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public int PlcPort { get; set; }
    public int ApiPort { get; set; }

    public ICollection<DataBlock> DataBlocks { get; set; } = [];
}
