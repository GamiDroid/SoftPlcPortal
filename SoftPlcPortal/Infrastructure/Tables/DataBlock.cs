using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftPlcPortal.Infrastructure.Tables;

public record DataBlock
{
    [Key] public Guid Key { get; set; }
    public Guid PlcConfigKey { get; set; }
    public int Number { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    //TODO: Create Save DataBlock command with Size property
    // so that database model is not misused.
    [NotMapped] public int Size { get; set; }


    public PlcConfig? PlcConfig { get; set; }
    public ICollection<DbField> DbFields { get; set; } = [];
}
