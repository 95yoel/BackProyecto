using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace AsturTravel.Models
{
    public class Destacados
    {
        public int Id { get; set; }
        public int ViajeId { get; set; }
        public Viajes? Viaje { get; set; }

        
        
    }
}
