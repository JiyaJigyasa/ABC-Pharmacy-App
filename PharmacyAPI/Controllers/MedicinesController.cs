using Microsoft.AspNetCore.Mvc;
using PharmacyAPI.Models;
using PharmacyAPI.Services;

namespace PharmacyAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MedicinesController : ControllerBase
{
    private readonly IMedicineService _medicineService;

    public MedicinesController(IMedicineService medicineService)
    {
        _medicineService = medicineService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Medicine>> GetAllMedicines()
    {
        try
        {
            var medicines = _medicineService.GetAllMedicines();
            return Ok(medicines);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error retrieving medicines", error = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public ActionResult<Medicine> GetMedicineById(int id)
    {
        try
        {
            var medicine = _medicineService.GetMedicineById(id);
            if (medicine == null)
            {
                return NotFound(new { message = $"Medicine with ID {id} not found" });
            }
            return Ok(medicine);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error retrieving medicine", error = ex.Message });
        }
    }

    [HttpGet("search")]
    public ActionResult<IEnumerable<Medicine>> SearchMedicines([FromQuery] string? name)
    {
        try
        {
            var medicines = _medicineService.SearchMedicines(name);
            return Ok(medicines);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error searching medicines", error = ex.Message });
        }
    }

    [HttpPost]
    public ActionResult<Medicine> AddMedicine([FromBody] Medicine medicine)
    {
        try
        {
            // Validation
            if (medicine == null)
            {
                return BadRequest(new { message = "Medicine data is required" });
            }

            if (string.IsNullOrWhiteSpace(medicine.FullName))
            {
                return BadRequest(new { message = "Medicine full name is required" });
            }

            if (medicine.Quantity < 0)
            {
                return BadRequest(new { message = "Quantity cannot be negative" });
            }

            if (medicine.Price < 0)
            {
                return BadRequest(new { message = "Price cannot be negative" });
            }

            if (medicine.ExpiryDate.Date < DateTime.Now.Date)
            {
                return BadRequest(new { message = "Expiry date cannot be in the past" });
            }

            var addedMedicine = _medicineService.AddMedicine(medicine);
            if (addedMedicine == null)
            {
                return StatusCode(500, new { message = "Error adding medicine" });
            }

            return CreatedAtAction(nameof(GetMedicineById), new { id = addedMedicine.Id }, addedMedicine);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error adding medicine", error = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public ActionResult<Medicine> UpdateMedicine(int id, [FromBody] Medicine medicine)
    {
        try
        {
            // Validation
            if (medicine == null)
            {
                return BadRequest(new { message = "Medicine data is required" });
            }

            if (string.IsNullOrWhiteSpace(medicine.FullName))
            {
                return BadRequest(new { message = "Medicine full name is required" });
            }

            if (medicine.Quantity < 0)
            {
                return BadRequest(new { message = "Quantity cannot be negative" });
            }

            if (medicine.Price < 0)
            {
                return BadRequest(new { message = "Price cannot be negative" });
            }

            if (medicine.ExpiryDate.Date < DateTime.Now.Date)
            {
                return BadRequest(new { message = "Expiry date cannot be in the past" });
            }

            var updatedMedicine = _medicineService.UpdateMedicine(id, medicine);
            if (updatedMedicine == null)
            {
                return NotFound(new { message = $"Medicine with ID {id} not found" });
            }

            return Ok(updatedMedicine);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error updating medicine", error = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteMedicine(int id)
    {
        try
        {
            var result = _medicineService.DeleteMedicine(id);
            if (!result)
            {
                return NotFound(new { message = $"Medicine with ID {id} not found" });
            }

            return Ok(new { message = $"Medicine with ID {id} deleted successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Error deleting medicine", error = ex.Message });
        }
    }
}