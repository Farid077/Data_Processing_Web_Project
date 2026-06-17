namespace WebProject.ViewModels;

public class RoleManagementVM
{
    public string Name { get; set; }
    public Dictionary<string, List<string>> Permissions { get; set; } = [];
    public ICollection<string> Users { get; set; } = [];
    //public string Department { get; set; }
}
