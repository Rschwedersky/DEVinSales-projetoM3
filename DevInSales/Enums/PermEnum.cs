using System.ComponentModel.DataAnnotations;

namespace DevInSales.Enums
{
    public enum PermEnum
    {
        [Display(Name = "Usuário")]
        Usuario = 1,
        [Display(Name = "Gerente")]
        Gerente = 2,
        [Display(Name = "Administrador")]
        Administrador = 3,
    }
}