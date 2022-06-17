using DevInSales.Enums;
using Microsoft.OpenApi.Extensions;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevInSales.Models;

public class Profile
{
    [Column("id")]
    public int Id { get; set; }
    [Column("Role")]
    public string Role { get; set; }
    [Column("Perm")]
    public PermEnum Perm { get; set; }

    public Profile()
    {
    }

    public Profile(int id, string role, PermEnum perm)
    {
        Id = id;
        Perm = perm;
        Role = role;
    }
}