using System.ComponentModel.DataAnnotations;

namespace AsturTravel.Models
{
    public class Reservas
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }
        
        public int ViajeId { get; set; }
        public Viajes? Viaje { get; set; }
        public DateTime? FechaReserva { get; set; }
        public DateTime? FechaPago { get; set; }
        public DateTime? FechaCancelacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public int? NumeroPersonas { get; set; }
        [DataType(DataType.Currency)]
        public decimal? Precio { get; set; }
        
        
    }
}
