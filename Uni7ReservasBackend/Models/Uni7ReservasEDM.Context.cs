﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Uni7ReservasEntities : DbContext
    {
        public Uni7ReservasEntities()
            : base("name=Uni7ReservasEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Usuario> Usuarios { get; set; }
        public virtual DbSet<Reserva> Reservas { get; set; }
        public virtual DbSet<Local> Locais { get; set; }
        public virtual DbSet<Equipamento> Equipamentos { get; set; }
        public virtual DbSet<CategoriaEquipamento> Categorias { get; set; }
        public virtual DbSet<Chamado> Chamados { get; set; }
        public virtual DbSet<Recurso> Recursos { get; set; }
        public virtual DbSet<RecursoLocal> RecursosLocais { get; set; }
    }
}
