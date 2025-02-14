using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using SoftPlcPortal.Infrastructure.Database;
using SoftPlcPortal.Infrastructure.Tables;

namespace SoftPlcPortal.Application.Services;

public class DbFieldsService(
    ILogger<DbFieldsService> logger,
    AppDbContext db)
{
    private readonly ILogger<DbFieldsService> _logger = logger;
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

    public async Task<byte[]> ExportDbFieldsToExcelAsync(Guid dataBlockKey, CancellationToken cancellationToken = default)
    {
        var dbFields = await _db.DbFields
            .AsNoTracking()
            .Where(x => x.DataBlockKey == dataBlockKey)
            .ToListAsync(cancellationToken);

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("DbFields");

        // Add headers
        worksheet.Cell(1, 1).Value = "Name";
        worksheet.Cell(1, 2).Value = "DataType";
        worksheet.Cell(1, 3).Value = "ByteOffset";
        worksheet.Cell(1, 4).Value = "BitOffset";
        worksheet.Cell(1, 5).Value = "Comment";
        worksheet.Cell(1, 6).Value = "StartValue";

        // Add data
        for (int i = 0; i < dbFields.Count; i++)
        {
            var dbField = dbFields[i];
            worksheet.Cell(i + 2, 1).Value = dbField.Name;
            worksheet.Cell(i + 2, 2).Value = dbField.DataType.ToString();
            worksheet.Cell(i + 2, 3).Value = dbField.ByteOffset;
            worksheet.Cell(i + 2, 4).Value = dbField.BitOffset;
            worksheet.Cell(i + 2, 5).Value = dbField.Comment;
            worksheet.Cell(i + 2, 6).Value = dbField.StartValue;
        }

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }

    public async Task ImportDbFieldsFromExcelAsync(Guid dataBlockKey, string path, CancellationToken cancellationToken = default)
    {
        try
        {
            var fileContent = await File.ReadAllBytesAsync(path, cancellationToken);
            using var stream = new MemoryStream(fileContent);
            using var workbook = new XLWorkbook(stream);
            var worksheet = workbook.Worksheet("DbFields");

            var dbFields = new List<DbField>();
            var rows = worksheet.RowsUsed().Skip(1); // Skip header row

            foreach (var row in rows)
            {
                var dbField = new DbField
                {
                    Key = Guid.NewGuid(),
                    DataBlockKey = dataBlockKey,
                    Name = row.Cell(1).GetString(),
                    DataType = Enum.Parse<DbDataType>(row.Cell(2).GetString()),
                    ByteOffset = row.Cell(3).GetValue<int>(),
                    BitOffset = row.Cell(4).GetValue<int>(),
                    Comment = row.Cell(5).GetString(),
                    StartValue = row.Cell(6).GetString()
                };
                dbFields.Add(dbField);
            }

            _db.DbFields.AddRange(dbFields);
            await _db.SaveChangesAsync(cancellationToken);
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Error when importing db fields");
        }
        finally
        {
            File.Delete(path);
        }
    }
}
