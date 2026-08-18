using PharmacyAPI.Models;

namespace PharmacyAPI.Services;

public interface IMedicineService
{
    List<Medicine> GetAllMedicines();
    Medicine? GetMedicineById(int id);
    List<Medicine> SearchMedicines(string? name);
    Medicine? AddMedicine(Medicine medicine);
    Medicine? UpdateMedicine(int id, Medicine medicine);
    bool DeleteMedicine(int id);
}
