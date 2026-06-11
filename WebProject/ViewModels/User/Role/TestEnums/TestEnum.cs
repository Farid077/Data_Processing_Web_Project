namespace WebProject.ViewModels;

[Flags]
public enum TestEnum
{
    Read = 1,
    Create = 2,
    Update = 4,
    Delete = 8,
    Options = 16,
    DeleteAll = 32,
    Import = 64,
    Export = 128,
    
}
