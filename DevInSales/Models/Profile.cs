using System.ComponentModel.DataAnnotations.Schema;

namespace DevInSales.Models;

public class Profile
{
    [Column("id")]
    public int Id { get; set; }
    [Column("name")]
    public string Name{ get; set; }
    [Column("role")]
    public string Role { get; set; }

    public Profile()
    {
    }

    public Profile(int id, string name, string role)
    {
        Id = id;
        Name = name;
        Role = role;
    }
}