namespace PharmacyAPI.Models;

public class Medicine
{
    public int Id { get; set; }
    public string? FullName { get; set; }
    public string? Notes { get; set; }
    public DateTime ExpiryDate { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }  // 2 decimal places
    public string? Brand { get; set; }
}