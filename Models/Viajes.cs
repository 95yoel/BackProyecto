using System.ComponentModel.DataAnnotations;

namespace AsturTravel.Models
{
    public class Viajes
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100), MinLength(3)]
        public string? Nombre { get; set; }

        public int DestinoId { get; set; }
        public Destinos? Destino { get; set; }
        public int TipoViajeId { get; set; }
        public TiposViaje? TipoViaje { get; set; }
        public DateTime? FechaSalida { get; set; }
        public DateTime? FechaLlegada { get; set; }
        [DataType(DataType.Currency)]
        public decimal? Precio { get; set; }

    }
}
