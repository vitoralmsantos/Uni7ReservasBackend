//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Uni7ReservasBackend.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Reserva
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Reserva()
        {
            this.CategoriasEquipamentos = new HashSet<CategoriaEquipamento>();
        }
    
        public int Id { get; set; }
        public System.DateTime Data { get; set; }
        public System.DateTime ReservadoEm { get; set; }
        public string Obs { get; set; }
        public string Horario { get; set; }
        public string Turno { get; set; }
    
        public virtual Usuario Usuario { get; set; }
        public virtual Local Local { get; set; }
        public virtual Controle Controle { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CategoriaEquipamento> CategoriasEquipamentos { get; set; }
    }
}
