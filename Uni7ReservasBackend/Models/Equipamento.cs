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
    
    public partial class Equipamento
    {
        public int Id { get; set; }
        public string Modelo { get; set; }
        public bool Disponivel { get; set; }
    
        public virtual CategoriaEquipamento CategoriaEquipamento { get; set; }
    }
}
