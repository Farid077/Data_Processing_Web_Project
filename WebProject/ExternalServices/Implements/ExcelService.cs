using ClosedXML.Excel;
using WebProject.Models;

namespace WebProject.Services;

public interface IExcelService
{
    Task<ExcelData> ImportAsync(IFormFile file);
    byte[] Export(ExcelData data, List<ExcelRow> rows);
}

public class ExcelService : IExcelService
{
    public async Task<ExcelData> ImportAsync(IFormFile file)
    {
        var data = new ExcelData
        {
            FileName = file.FileName,
            ImportedAt = DateTime.Now
        };

        using var stream = new MemoryStream();
        await file.CopyToAsync(stream);
        stream.Position = 0;

        using var workbook = new XLWorkbook(stream);
        var sheet = workbook.Worksheets.First();
        var rows = sheet.RangeUsed()?.RowsUsed().ToList();

        if (rows == null || rows.Count == 0) return data;

        // First row = headers
        var headerRow = rows[0];
        foreach (var cell in headerRow.Cells())
        {
            var header = cell.GetString().Trim();
            if (!string.IsNullOrEmpty(header))
                data.Headers.Add(header);
        }

        // Data rows
        for (int i = 1; i < rows.Count; i++)
        {
            var excelRow = new ExcelRow();
            var cells = rows[i].Cells(1, data.Headers.Count).ToList();
            for (int j = 0; j < data.Headers.Count; j++)
            {
                var val = j < cells.Count ? GetCellValue(cells[j]) : "";
                excelRow.Cells[data.Headers[j]] = val;
            }
            data.Rows.Add(excelRow);
        }

        return data;
    }

    private static string GetCellValue(IXLCell cell)
    {
        return cell.DataType switch
        {
            XLDataType.DateTime => cell.GetDateTime().ToString("yyyy-MM-dd"),
            XLDataType.Number => cell.GetDouble().ToString(),
            XLDataType.Boolean => cell.GetBoolean().ToString(),
            _ => cell.GetString()
        };
    }

    public byte[] Export(ExcelData data, List<ExcelRow> rows)
    {
        using var workbook = new XLWorkbook();
        var sheet = workbook.Worksheets.Add("Data");

        // Header row
        for (int i = 0; i < data.Headers.Count; i++)
        {
            var cell = sheet.Cell(1, i + 1);
            cell.Value = data.Headers[i];
            cell.Style.Font.Bold = true;
            cell.Style.Fill.BackgroundColor = XLColor.FromArgb(31, 73, 125);
            cell.Style.Font.FontColor = XLColor.White;
            cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        }

        // Data rows
        for (int i = 0; i < rows.Count; i++)
        {
            for (int j = 0; j < data.Headers.Count; j++)
            {
                var val = rows[i].Cells.TryGetValue(data.Headers[j], out var v) ? v : "";
                sheet.Cell(i + 2, j + 1).Value = val;
            }

            if (i % 2 == 1)
            {
                sheet.Row(i + 2).Style.Fill.BackgroundColor = XLColor.FromArgb(242, 242, 242);
            }
        }

        sheet.ColumnsUsed().AdjustToContents();
        sheet.RangeUsed()?.SetAutoFilter();

        // Freeze header
        sheet.SheetView.Freeze(1, 0);

        using var ms = new MemoryStream();
        workbook.SaveAs(ms);
        return ms.ToArray();
    }
}
