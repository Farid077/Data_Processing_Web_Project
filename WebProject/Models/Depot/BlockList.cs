namespace WebProject.Models;

public class BlockList
{
    public string Key { get; set; }
    public ICollection<string> Value { get; set; } = [];
}
