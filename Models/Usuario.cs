using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

[Index(nameof(Email), IsUnique = true)]
public class Usuario
{
    public int Id { get; set; }

    [Required]
    public string Nome { get; set; }

    [Required]
    public string Email { get; set; }

    [Required]
    public string Senha { get; set; }

    [Required]
    public int Tipo { get; set; } //1 admin, 2 usuário padrão
}