using System.ComponentModel.DataAnnotations;

namespace WebProject.ViewModels;

public class IssueUpdateVM
{
    public int Id { get; set; }
    public string Category { get; set; }
    public string SubCategory { get; set; }

    [MaxLength(64, ErrorMessage = "Note cannot be include more than 64 characters.")]
    public string? Note { get; set; } = "";
    public string Status { get; set; }
    public IReadOnlyCollection<string> Statuses { get; set; } = [];
    public IReadOnlyCollection<string> Categories { get; set; } = [];
    public IReadOnlyCollection<string> SubCategories { get; set; } = [];
}
