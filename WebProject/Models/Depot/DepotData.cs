namespace WebProject.Models;

public class DepotData : BaseEntity
{
    public int Id { get; set; }
    public int? SN { get; set; } // Sıra Nömrəsi
    public string? DQN { get; set; } // Dövlət Qeydiyyat Nişanı
    public string? EyNom { get; set; } //Eyniləşdirmə Nömrəsi
    public string? DVR { get; set; } // DVR Versiya Nömrəsi
    public string? CD { get; set; } // Connect - Disconnect
    public string? QapiR { get; set; } // Qapı və R
    public string? SayKam { get; set; } // Sayım Kameraları
    public string? HDDV { get; set; } // HDD Vəziyyəti -> işlək - nasaz
    public string? HDDH { get; set; } // HDD Həcmi
    public string? HDDSM { get; set; } // HDD Saxlama Müddəti
    public string? DVRV { get; set; } // DVR Vəziyyəti -> işlək - nasaz
    public string? Kam { get; set; } // Kamera Yeri
    public string? KamV { get; set; } // Kamera Vəziyyəti -> işlək - nasaz
    public string? KamNom { get; set; } // Kamera Nömrəsi
    public string? SalMon { get; set; } // Salon Monitoru
    public string? DaySes { get; set; } // Dayanacaq Səsləndirməsi
    public string? SurMik { get; set; } // Sürücü Mikrofonu Vəziyyəti -> işlək - nasaz
    public string? Trafared { get; set; } // Trafared Vəziyyəti -> işlək - nasaz
    public string? Qeyd { get; set; } // Qeyd
    public bool IsConfirmed { get; set; } = false;
    public DateTime? ConfirmedDate { get; set; } // Təsdiqləndiyi Tarix
    public string? ConfirmerId { get; set; } // Təsdiqləyən Şəxs
    public User? User { get; set; }
    public int? Depot { get; set; } // Depo Nömrəsi
}
