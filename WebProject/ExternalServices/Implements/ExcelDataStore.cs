using WebProject.Models;

namespace WebProject.Services;

public interface IExcelDataStore
{
    void SetData(ExcelData data);
    ExcelData? GetData();
    void UpdateRow(int rowId, Dictionary<string, string> cells);
    void DeleteRow(int rowId);
    void AddRow(Dictionary<string, string> cells);
}

public class ExcelDataStore : IExcelDataStore
{
    private ExcelData? _data;
    private int _nextId = 1;
    private readonly object _lock = new();

    public void SetData(ExcelData data)
    {
        lock (_lock)
        {
            _nextId = 1;
            foreach (var row in data.Rows)
                row.Id = _nextId++;
            _data = data;
        }
    }

    public ExcelData? GetData()
    {
        lock (_lock) return _data;
    }

    public void UpdateRow(int rowId, Dictionary<string, string> cells)
    {
        lock (_lock)
        {
            var row = _data?.Rows.FirstOrDefault(r => r.Id == rowId);
            if (row != null) row.Cells = cells;
        }
    }

    public void DeleteRow(int rowId)
    {
        lock (_lock)
        {
            _data?.Rows.RemoveAll(r => r.Id == rowId);
        }
    }

    public void AddRow(Dictionary<string, string> cells)
    {
        lock (_lock)
        {
            if (_data == null) return;
            _data.Rows.Add(new ExcelRow { Id = _nextId++, Cells = cells });
        }
    }
}
