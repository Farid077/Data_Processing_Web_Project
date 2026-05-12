namespace WebProject.Models;

public class DepotData
{
    public int Id { get; set; }
    public int SN { get; set; }
    public string DQN { get; set; }
    public string EyNom { get; set; }
    public string DVR { get; set; }
    public string? CD { get; set; }
    public string? QapiR { get; set; }
    public string? SayKam { get; set; }
    public string? HDDSt { get; set; }
    public string? HDDHc { get; set; }
    public string? HDDSM { get; set; }
    public string? DVRSt { get; set; }
    public string? Kam { get; set; }
    public string? KamSt { get; set; }
    public string? KamNom { get; set; }
    public string? SalMon { get; set; }
    public string? DaySes { get; set; }
    public string? SurMik { get; set; }
    public string? Trafared { get; set; }
    public string? Note { get; set; }
    public DateTime? ConfirmedDate { get; set; }
    public bool IsConfirmed { get; set; } = false;
    public bool IsDeleted { get; set; } = false;
    public string? ApproverId { get; set; }
    public User? User { get; set; }
}
