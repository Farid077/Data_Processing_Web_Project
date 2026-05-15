namespace WebProject.Models;

public class ExcelRow
{
    public int Id { get; set; }
    public Dictionary<string, string> Cells { get; set; } = new();
}

public class ExcelData
{
    public List<string> Headers { get; set; } = new();
    public List<ExcelRow> Rows { get; set; } = new();
    public string? FileName { get; set; }
    public DateTime? ImportedAt { get; set; }
}

public class GridRequest
{
    public string? SearchTerm { get; set; }
    public string? SortColumn { get; set; }
    public string SortDirection { get; set; } = "asc";
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public Dictionary<string, string> ColumnFilters { get; set; } = new();
}

public class GridResponse
{
    public List<string> Headers { get; set; } = new();
    public List<ExcelRow> Rows { get; set; } = new();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public string? FileName { get; set; }
}

public class EditRowRequest
{
    public int RowId { get; set; }
    public Dictionary<string, string> Cells { get; set; } = new();
}
