namespace WebProject.Models;

[Flags]
public enum PageAccess
{
    Read = 1,
    Create = 2,
    Update = 4,
    Delete = 8,
    DeleteAll = 16,
    Block = 32,
    Options = 64,
    Import = 128,
    Export = 256,
    FullAccess = Read | Create | Update | Delete | DeleteAll | Options | Import | Export,
}
