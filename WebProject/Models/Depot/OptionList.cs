namespace WebProject.Models;

public class OptionList
{
    public string Key { get; set; }
    public ICollection<string> Value { get; set; } = [];
}
