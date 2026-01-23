namespace CoreEntity.Models;

public class Company {
    public int Id { get; set; }
    public string Name { get; set; }  = string.Empty;
    public Address Address { get; set; } = null!;
}