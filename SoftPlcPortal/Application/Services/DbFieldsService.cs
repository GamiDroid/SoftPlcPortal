using Microsoft.EntityFrameworkCore;
using SoftPlcPortal.Infrastructure.Database;
using SoftPlcPortal.Infrastructure.Tables;

namespace SoftPlcPortal.Application.Services;

public class DbFieldsService(AppDbContext db)
{
    private readonly AppDbContext _db = db;

    public Task<List<DbField>> GetAllAsync(Guid dataBlockKey, CancellationToken cancellationToken = default)
    {
        return _db.DbFields
            .AsNoTracking()
            .Where(x => x.DataBlockKey == dataBlockKey)
            .OrderBy(x => x.ByteOffset).ThenBy(x => x.BitOffset)
            .ToListAsync(cancellationToken);
    }

    public async Task<DbField> CreateAsync(Guid dataBlockKey, DbField dbField)
    {
        dbField.Key = Guid.NewGuid();
        dbField.DataBlockKey = dataBlockKey;

        _db.DbFields.Add(dbField);
        await _db.SaveChangesAsync();

        return dbField;
    }

    public async Task<DbField> UpdateAsync(DbField dbField)
    {
        var existingDbField = await _db.DbFields.FirstOrDefaultAsync(x => x.Key == dbField.Key);
        if (existingDbField is null)
        {
            throw new InvalidOperationException();
        }

        existingDbField.Name = dbField.Name;    
        existingDbField.DataType = dbField.DataType;
        existingDbField.ByteOffset = dbField.ByteOffset;
        existingDbField.BitOffset = dbField.BitOffset;
        existingDbField.StartValue = dbField.StartValue;
        existingDbField.Comment = dbField.Comment;

        await _db.SaveChangesAsync();
        return dbField;
    }
}
