using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;


namespace USF_Health_MVC_EF.Models
{
    public partial class USF_Health_MVC_EFContext : DbContext
    {
        public USF_Health_MVC_EFContext()
        {
        }

        public USF_Health_MVC_EFContext(DbContextOptions<USF_Health_MVC_EFContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Individual> tb_individuals { get; set; }
        public virtual DbSet<IndividualSample> tb_individuals_samples { get; set; }
        public virtual DbSet<Login> tb_login { get; set; }
        public virtual DbSet<Pool> tb_pools { get; set; }
        public virtual DbSet<SpIndividuals> usp_individual_select { get; set; }
        public virtual DbSet<SpIndividualsSamples> usp_individuals_samples_select { get; set; }
        public virtual DbSet<SpPools> usp_pools_select { get; set; }
        public virtual DbSet<SpStudies> usp_studies_select { get; set; }
        public virtual DbSet<SpReferences> usp_references_select { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Individual>
            (entity =>
            {
                entity.HasKey(e => e.ind_id)
                    .HasName("pk_tb_individuals");

                entity.Property(e => e.ind_date_created)
                    .HasColumnType("date");

                entity.Property(e => e.ind_time_created)
                    .HasColumnType("time");

                entity.Property(e => e.usr_id_created)
                    .HasColumnType("int");

                entity.Property(e => e.ind_first_name)
                    .HasColumnType("varchar(800)")
                    .HasMaxLength(800)
                    .IsUnicode(false);

                entity.Property(e => e.ind_last_name)
                    .HasColumnType("varchar(800)")
                    .HasMaxLength(800)
                    .IsUnicode(false);

                entity.Property(e => e.ind_gender)
                    .HasColumnType("char(1)")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.ind_document)
                    .HasColumnType("varchar(800)")
                    .HasMaxLength(800)
                    .IsUnicode(false);

                entity.Property(e => e.ind_email)
                    .HasColumnType("varchar(800)")
                    .HasMaxLength(800)
                    .IsUnicode(false);

                entity.Property(e => e.ind_phone)
                    .HasColumnType("varchar(800)")
                    .HasMaxLength(800)
                    .IsUnicode(false);

                entity.Property(e => e.ind_birthdate)
                    .HasColumnType("date");

                entity.Property(e => e.ref_id)
                    .HasColumnType("int");

                entity.Property(e => e.std_id)
                    .HasColumnType("int");
    

            });

            modelBuilder.Entity<IndividualSample>
          (entity =>
          {
              entity.HasKey(e => e.is_id)  
                  .HasName("pk_tb_individuals_samples");

              entity.Property(e => e.is_barcode)
                  .HasColumnType("varchar(8)")
                  .HasMaxLength(8)
                  .IsUnicode(false);

              entity.Property(e => e.is_date_created)
                    .HasColumnType("date")
                    .HasDefaultValueSql("GETDATE()");

              entity.Property(e => e.is_time_created)
                     .HasColumnType("time")
                     .HasDefaultValueSql("GETDATE()");

              entity.Property(e => e.usr_id_created)
                    .HasColumnType("int");

              entity.Property(e => e.is_date_collected)
                    .HasColumnType("date")
                    .HasDefaultValueSql("GETDATE()");

              entity.Property(e => e.is_time_collected)
                     .HasColumnType("time")
                     .HasDefaultValueSql("GETDATE()");

              entity.Property(e => e.usr_id_collected)
                     .HasColumnType("int");

              entity.Property(e => e.is_date_registered)
                    .HasColumnType("date")
                    .HasDefaultValueSql("GETDATE()");

              entity.Property(e => e.is_time_registered)
                     .HasColumnType("time")
                     .HasDefaultValueSql("GETDATE()");

              entity.Property(e => e.usr_id_registered)
                    .HasColumnType("int");

              entity.Property(e => e.ind_id)
                   .HasColumnType("int");

              entity.Property(e => e.poo_id)
                   .HasColumnType("int");

              entity.Property(e => e.is_date_registered_pool)
                   .HasColumnType("date")
                   .HasDefaultValueSql("GETDATE()");

              entity.Property(e => e.is_time_registered_pool)
                    .HasColumnType("time")
                    .HasDefaultValueSql("GETDATE()");

              entity.Property(e => e.usr_id_registered_pool)
                    .HasColumnType("int");

              entity.Property(e => e.is_well_number)
                  .HasColumnType("varchar(800)")
                  .IsUnicode(false);

              entity.Property(e => e.is_details)
                  .HasColumnType("varchar(max)")
                  .IsUnicode(false);
            });



            modelBuilder.Entity<Pool>
            (entity =>
            {
                entity.HasKey(e => e.poo_id)
                    .HasName("pk_tb_pools");

                entity.Property(e => e.poo_date_created)
                      .HasColumnType("date")
                      .HasDefaultValueSql("GETDATE()");

                entity.Property(e => e.poo_time_created)
                       .HasColumnType("time")
                       .HasDefaultValueSql("GETDATE()");

                entity.Property(e => e.poo_details)
                    .HasColumnType("varchar(max)")
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Login>
            (entity =>
            {
                entity.HasNoKey().ToView(null);
            });



            modelBuilder.Entity<SpPools> //().HasNoKey().ToView(null)
            (entity =>
            {
                entity.HasNoKey().ToView(null);
            });

            modelBuilder.Entity<SpIndividualsSamples> //().HasNoKey().ToView(null)
            (entity =>
            {
                entity.HasNoKey().ToView(null);
            });

            modelBuilder.Entity<SpIndividuals> //().HasNoKey().ToView(null)
            (entity =>
            {
                entity.HasNoKey().ToView(null);
            });

            modelBuilder.Entity<SpStudies> //().HasNoKey().ToView(null)
            (entity =>
            {
                entity.HasNoKey().ToView(null);
            });

            modelBuilder.Entity<SpReferences> //().HasNoKey().ToView(null)
            (entity =>
            {
                entity.HasNoKey().ToView(null);
            });


            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
