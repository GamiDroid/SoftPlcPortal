using Microsoft.EntityFrameworkCore;
using SoftPlcPortal.Infrastructure.Database;
using SoftPlcPortal.Infrastructure.SoftPlc;
using SoftPlcPortal.Infrastructure.Tables;

namespace SoftPlcPortal.Application.Services;

public class DataBlocksService(
    AppDbContext db,
    SoftPlcClientFactory softPlcClientFactory)
{
    private readonly AppDbContext _db = db;
    private readonly SoftPlcClientFactory _softPlcClientFactory = softPlcClientFactory;

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
        var config = await _db.PlcConfigs
            .Where(x => x.Key == dataBlock.PlcConfigKey)
            .Select(x => new { x.Address, x.ApiPort })
            .FirstOrDefaultAsync(cancellationToken) ?? 
            throw new InvalidDataException("dataBlock is not linked to a PLC config");

        var host = $"http://{config.Address}:{config.ApiPort}";
        var softPlcClient = _softPlcClientFactory.Create(host);

        await softPlcClient.CreateDataBlockAsync(dataBlock.Number, dataBlock.Size, cancellationToken);

        dataBlock.Key = Guid.NewGuid();

        _db.DataBlocks.Add(dataBlock);
        await _db.SaveChangesAsync(cancellationToken);

        return dataBlock;
    }

    // update a datablock
    public async Task<DataBlock> UpdateAsync(DataBlock dataBlock, CancellationToken cancellationToken = default)
    {

        var config = await _db.PlcConfigs
            .Where(x => x.Key == dataBlock.PlcConfigKey)
            .Select(x => new { x.Address, x.ApiPort })
            .FirstOrDefaultAsync(cancellationToken) ??
            throw new InvalidDataException("dataBlock is not linked to a PLC config");

        var host = $"http://{config.Address}:{config.ApiPort}";
        var softPlcClient = _softPlcClientFactory.Create(host);

        await softPlcClient.DeleteDataBlockAsync(dataBlock.Number, cancellationToken);
        await softPlcClient.CreateDataBlockAsync(dataBlock.Number, dataBlock.Size, cancellationToken);

        var dto = await _db.DataBlocks.FirstOrDefaultAsync(x => x.Key == dataBlock.Key, cancellationToken) 
            ?? throw new InvalidDataException("Unable to find DataBlock");

        dto.Number = dataBlock.Number;
        dto.Name = dataBlock.Name;
        dto.Description = dataBlock.Description;

        await _db.SaveChangesAsync(cancellationToken);

        return dto;
    }

    public async Task<DataBlockData> GetDataAsync(Guid key, CancellationToken cancellationToken = default)
    {
        var dataBlockConfig = await _db.DataBlocks
            .Where(x => x.Key == key)
            .Select(x => new
            {
                x.Number,
                x.PlcConfig!.Address,
                x.PlcConfig!.ApiPort
            })
            .FirstOrDefaultAsync(cancellationToken) ??
            throw new ArgumentException($"DataBlock with key {key} not found", nameof(key));

        var host = $"http://{dataBlockConfig.Address}:{dataBlockConfig.ApiPort}";

        var softPlcClient = _softPlcClientFactory.Create(host);

        var dataBlockResponse = await softPlcClient.GetDataBlockByIdAsync(dataBlockConfig.Number, cancellationToken);

        var data = Convert.FromBase64String(dataBlockResponse.Data);

        return new DataBlockData(dataBlockResponse.Size, data);
    }
}

public record DataBlockData(int Size, byte[] Data);
