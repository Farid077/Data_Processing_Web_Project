namespace WebProject.Models;

public enum Pages
{
    Users = 4,
    Roles = 8,
    Options = 16,
    Depo1 = 32,
    Depo2 = 64,
    Depo3 = 128,
    Depo4 = 256,
    Depo5 = 512,
    AllDepos = Depo1 | Depo2 | Depo3 | Depo4 | Depo5,
}
