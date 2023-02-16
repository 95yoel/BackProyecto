using System.ComponentModel.DataAnnotations;

namespace AsturTravel.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50), MinLength(3)]
        public string Nombre { get; set; }
        [Required]
        [StringLength(100), MinLength(3)]
        public string Apellidos { get; set; }
        [Required]
        [StringLength(100), MinLength(3)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Contrasenas { get; set; }
        public Roles Rol { get; set; }
        
    }
}
