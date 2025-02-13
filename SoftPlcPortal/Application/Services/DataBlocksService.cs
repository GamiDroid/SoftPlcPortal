using Microsoft.EntityFrameworkCore;
using SoftPlcPortal.Infrastructure.Database;
using SoftPlcPortal.Infrastructure.Tables;

namespace SoftPlcPortal.Application.Services;

public class DataBlocksService(AppDbContext db)
{
    private readonly AppDbContext _db = db;

    //get all datablocks of a plcConfigKey
    public Task<List<DataBlock>> GetAllAsync(Guid plcConfigKey, CancellationToken cancellationToken = default)
    {
        return _db.DataBlocks.AsNoTracking().Where(x => x.PlcConfigKey == plcConfigKey).ToListAsync(cancellationToken);
    }

    // get one datablock by id
    public Task<DataBlock?> GetByIdAsync(Guid key, CancellationToken cancellationToken = default)
    {
        return _db.DataBlocks.AsNoTracking().FirstOrDefaultAsync(x => x.Key == key, cancellationToken);
    }

    // Add a new datablock
    public async Task<DataBlock> CreateAsync(DataBlock dataBlock, CancellationToken cancellationToken = default)
    {
        dataBlock.Key = Guid.NewGuid();

        _db.DataBlocks.Add(dataBlock);
        await _db.SaveChangesAsync(cancellationToken);

        return dataBlock;
    }

    // update a datablock
    public async Task<DataBlock> UpdateAsync(DataBlock dataBlock, CancellationToken cancellationToken = default)
    {
        var dto = await _db.DataBlocks.FirstOrDefaultAsync(x => x.Key == dataBlock.Key, cancellationToken);

        if (dto is null)
            throw new InvalidDataException("Unable to find DataBlock");

        dto.Number = dataBlock.Number;
        dto.Name = dataBlock.Name;
        dto.Description = dataBlock.Description;

        await _db.SaveChangesAsync(cancellationToken);

        return dto;
    }
}
