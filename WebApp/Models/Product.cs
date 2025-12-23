namespace WebApp.Models;

public class Product
{
    public int Id { get; set; }
    public byte CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Content { get; set; } = null!;    
    public decimal Price { get; set; }
    public short Quantity { get; set; }
    public decimal? SaleOff { get; set; }
    public string ImageUrl { get; set; } = null!;
}