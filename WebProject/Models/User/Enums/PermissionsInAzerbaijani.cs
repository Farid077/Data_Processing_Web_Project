namespace WebProject.Models;

[Flags]
public enum PermissionsInAzerbaijani
{
    Baxış = 1,
    Yaratma = 2,
    Redaktə_etmə = 4,
    Silmə = 8,
    Hamısını_Silmə = 16,
    Sətir_Sütun_Kilidləmə = 32,
    Seçimlər = 64,
    İdxal_Excel = 128,
    İxrac_Excel = 256,
    Tam_İcazə = Baxış | Yaratma | Redaktə_etmə | Silmə | Hamısını_Silmə | Sətir_Sütun_Kilidləmə | Seçimlər | İdxal_Excel | İxrac_Excel
}
