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
    
    public partial class Local
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Local()
        {
            this.Reservas = new HashSet<Reserva>();
            this.RestricoesCategoriaEquipamento = new HashSet<CategoriaEquipamento>();
            this.Recursos = new HashSet<RecursoLocal>();
        }
    
        public int Id { get; set; }
        public string Nome { get; set; }
        public bool Reservavel { get; set; }
        public bool Disponivel { get; set; }
        public TIPOLOCAL Tipo { get; set; }
        public string ComentarioReserva { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Reserva> Reservas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CategoriaEquipamento> RestricoesCategoriaEquipamento { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RecursoLocal> Recursos { get; set; }
    }
}
