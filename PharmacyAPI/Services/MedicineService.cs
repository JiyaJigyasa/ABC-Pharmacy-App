using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using PharmacyAPI.Models;

namespace PharmacyAPI.Services;

public class MedicineService : IMedicineService
{
    private readonly string _filePath;
    private readonly string _dataFolder;
    private static readonly object _fileLock = new();
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };

    public MedicineService()
    {
        // Set up the path to medicines.json
        _dataFolder = Path.Combine(AppContext.BaseDirectory, "Data");
        _filePath = Path.Combine(_dataFolder, "medicines.json");

        // Create Data folder if it doesn't exist
        if (!Directory.Exists(_dataFolder))
        {
            Directory.CreateDirectory(_dataFolder);
        }

        // Create initial medicines.json if it doesn't exist
        if (!File.Exists(_filePath))
        {
            InitializeJsonFile();
        }
    }

    // Initialize medicines.json with empty array
    private void InitializeJsonFile()
    {
        File.WriteAllText(_filePath, "[]");
    }

    // Get all medicines
    public List<Medicine> GetAllMedicines()
    {
        try
        {
            lock (_fileLock)
            {
                var json = File.ReadAllText(_filePath);
                var medicines = JsonSerializer.Deserialize<List<Medicine>>(json, _jsonOptions) ?? new List<Medicine>();
                return medicines;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading medicines: {ex.Message}");
            return new List<Medicine>();
        }
    }

    // Get medicine by ID
    public Medicine? GetMedicineById(int id)
    {
        var medicines = GetAllMedicines();
        return medicines.FirstOrDefault(m => m.Id == id);
    }

    // Search medicines by name
    public List<Medicine> SearchMedicines(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return GetAllMedicines();
        }

        var medicines = GetAllMedicines();
        return medicines
            .Where(m => m.FullName != null && m.FullName.Contains(name, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    // Add new medicine
    public Medicine? AddMedicine(Medicine medicine)
    {
        try
        {
            lock (_fileLock)
            {
                var medicines = GetAllMedicinesUnlocked();

                // Generate new ID
                medicine.Id = medicines.Count > 0 ? medicines.Max(m => m.Id) + 1 : 1;

                medicines.Add(medicine);
                SaveMedicinesUnlocked(medicines);

                return medicine;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding medicine: {ex.Message}");
            return null;
        }
    }

    // Update existing medicine
    public Medicine? UpdateMedicine(int id, Medicine updatedMedicine)
    {
        try
        {
            lock (_fileLock)
            {
                var medicines = GetAllMedicinesUnlocked();
                var existingMedicine = medicines.FirstOrDefault(m => m.Id == id);

                if (existingMedicine == null)
                {
                    return null;
                }

                // Update properties
                existingMedicine.FullName = updatedMedicine.FullName;
                existingMedicine.Notes = updatedMedicine.Notes;
                existingMedicine.ExpiryDate = updatedMedicine.ExpiryDate;
                existingMedicine.Quantity = updatedMedicine.Quantity;
                existingMedicine.Price = updatedMedicine.Price;
                existingMedicine.Brand = updatedMedicine.Brand;

                SaveMedicinesUnlocked(medicines);

                return existingMedicine;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating medicine: {ex.Message}");
            return null;
        }
    }

    // Delete medicine by ID
    public bool DeleteMedicine(int id)
    {
        try
        {
            lock (_fileLock)
            {
                var medicines = GetAllMedicinesUnlocked();
                var medicineToRemove = medicines.FirstOrDefault(m => m.Id == id);

                if (medicineToRemove == null)
                {
                    return false;
                }

                medicines.Remove(medicineToRemove);
                SaveMedicinesUnlocked(medicines);

                return true;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting medicine: {ex.Message}");
            return false;
        }
    }

    // Reads medicines without acquiring _fileLock; caller must already hold it
    private List<Medicine> GetAllMedicinesUnlocked()
    {
        var json = File.ReadAllText(_filePath);
        return JsonSerializer.Deserialize<List<Medicine>>(json, _jsonOptions) ?? new List<Medicine>();
    }

    // Save medicines to JSON file; caller must already hold _fileLock
    private void SaveMedicinesUnlocked(List<Medicine> medicines)
    {
        try
        {
            var json = JsonSerializer.Serialize(medicines, _jsonOptions);
            File.WriteAllText(_filePath, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving medicines: {ex.Message}");
            throw;
        }
    }
}