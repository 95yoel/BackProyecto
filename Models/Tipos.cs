using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace AsturTravel.Models
{
    public enum Tipos
    {
        [Display(Name = "España")]
        Espana,
        [Display(Name = "Cruceros")]
        Cruceros,
        [Display(Name = "Diversión")]
        Diversion,
        [Display(Name = "Temporada")]
        Temporada,
        [Display(Name = "Europa")]
        Europa
    }
}
