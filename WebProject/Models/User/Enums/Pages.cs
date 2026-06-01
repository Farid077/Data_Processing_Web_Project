namespace WebProject.Models;

public enum Pages
{
    Users = 4,
    Roles = 8,
    Depo1 = 16,
    Depo2 = 32,
    Depo3 = 64,
    Depo4 = 128,
    Depo5 = 256,
    AllDepos = Depo1 | Depo2 | Depo3 | Depo4 | Depo5,
}
