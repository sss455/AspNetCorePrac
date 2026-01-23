namespace CoreEntity.Models;

public class Employee {
    public int Id { get; set; }
    public string FirstName { get; set; }  = string.Empty;
    public string LastName { get; set; }  = string.Empty;
    public Address Address { get; set; } = null!;
}