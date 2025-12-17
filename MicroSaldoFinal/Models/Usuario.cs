using System.ComponentModel.DataAnnotations;

namespace MicroSaldoFinal.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required, StringLength(120)]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(120)]
        public string Apellidos { get; set; } = string.Empty;

        [StringLength(50)]
        public string Documento { get; set; } = string.Empty;

        [StringLength(20)]
        public string Telefono { get; set; } = string.Empty;

        [Required, EmailAddress, StringLength(180)]
        public string Email { get; set; } = string.Empty;

        // Se almacena el hash, no el password plano
        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [StringLength(255)]
        public string RutaFoto { get; set; } = "/images/user.png";
    }
}
