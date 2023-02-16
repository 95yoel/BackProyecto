using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AsturTravel.Models
{
    public class Destinos
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50), MinLength(3)]
        public string Nombre { get; set; }
        [Required]
        [StringLength(50), MinLength(3)]
        public string Descripcion { get; set; }
        public string? Imagen { get; set; }
        [NotMapped]
        [Required]
        public IFormFile ImagenFile { get; set; }

    }
}
