using Microsoft.EntityFrameworkCore;
 using SoftPlcPortal.Infrastructure.Database;
using SoftPlcPortal.Infrastructure.Tables;

namespace SoftPlcPortal.Application.Services;

public class PlcConfigService(AppDbContext db)
{
    private readonly AppDbContext _db = db;

    public async Task<List<PlcConfig>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var plcConfigs = await _db.PlcConfigs.AsNoTracking().ToListAsync(cancellationToken);
        return plcConfigs;
    }

    public async Task<PlcConfig?> GetByKeyAsync(Guid key, CancellationToken cancellationToken = default)
    {
        var item = await _db.PlcConfigs
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Key == key, cancellationToken);
        if (item == null)
            return null;
        return item;
    }

    public async Task<PlcConfig> CreateAsync(PlcConfig plcConfig, CancellationToken cancellationToken = default)
    {
        plcConfig.Key = Guid.NewGuid();

        _db.PlcConfigs.Add(plcConfig);
        await _db.SaveChangesAsync(cancellationToken);

        return plcConfig;
    }

    public async Task<PlcConfig> UpdateAsync(PlcConfig plcConfig, CancellationToken cancellationToken = default)
    {
        var dto = await _db.PlcConfigs.FirstOrDefaultAsync(x => x.Key == plcConfig.Key, cancellationToken);

        if (dto is null)
            throw new InvalidDataException("Unable to find PlcConfig");

        dto.Name = plcConfig.Name;
        dto.Address = plcConfig.Address;
        dto.PlcPort = plcConfig.PlcPort;
        dto.ApiPort = plcConfig.ApiPort;

        await _db.SaveChangesAsync(cancellationToken);

        return dto;
    }
}
