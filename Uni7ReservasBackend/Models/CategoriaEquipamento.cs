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
    
    public partial class CategoriaEquipamento
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CategoriaEquipamento()
        {
            this.Equipamentos = new HashSet<Equipamento>();
        }
    
        public int Id { get; set; }
        public string Nome { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Equipamento> Equipamentos { get; set; }
        public virtual Reserva Reservas { get; set; }
    }
}
