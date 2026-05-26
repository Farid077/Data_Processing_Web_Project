namespace WebProject.ViewModels;

public class OptionListsVM
{
    public ICollection<string>? CDOptions { get; set; } // Connect - Disconnect
    public ICollection<string>? QapiROptions { get; set; } // Qapı və R
    public ICollection<string>? SayKamOptions { get; set; } // Sayım Kameraları
    public ICollection<string>? HDDVOptions { get; set; } // HDD Vəziyyəti -> işlək - nasaz
    public ICollection<string>? HDDHOptions { get; set; } // HDD Həcmi
    public ICollection<string>? HDDSMOptions { get; set; } // HDD Saxlama Müddəti
    public ICollection<string>? DVRVOptions { get; set; } // DVR Vəziyyəti -> işlək - nasaz
    public ICollection<string>? KamOptions { get; set; } // Kamera Yeri
    public ICollection<string>? KamVOptions { get; set; } // Kamera Vəziyyəti -> işlək - nasaz
    public ICollection<string>? KamNomOptions { get; set; } // Kamera Nömrəsi
    public ICollection<string>? SalMonOptions { get; set; } // Salon Monitoru
    public ICollection<string>? DaySesOptions { get; set; } // Dayanacaq Səsləndirməsi
    public ICollection<string>? SurMikOptions { get; set; } // Sürücü Mikrofonu Vəziyyəti -> işlək - nasaz
    public ICollection<string>? TrafaredOptions { get; set; } // Trafared Vəziyyəti -> işlək - nasaz
    public ICollection<int>? DepotOptions { get; set; } // Depo Nömrəsi
}
