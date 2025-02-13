using System.ComponentModel.DataAnnotations;

namespace SoftPlcPortal.Infrastructure.Tables;

public record DataBlock
{
    [Key] public Guid Key { get; set; }
    public Guid PlcConfigKey { get; set; }
    public int Number { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public PlcConfig? PlcConfig { get; set; }
    public ICollection<DbField> DbFields { get; set; } = [];
}
