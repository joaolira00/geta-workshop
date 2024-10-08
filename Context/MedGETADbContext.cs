using MedGETA.Hospitals;
using MedGETA.Patients;
using MedGETA.Records;
using Microsoft.EntityFrameworkCore;

namespace MedGETA.Context
{
    public class MedGETADbContext : DbContext
    {
        public MedGETADbContext(DbContextOptions<MedGETADbContext> options) : base(options)
        {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //N:N between Hospital and Patients
            modelBuilder.Entity<Hospital>()
            .HasMany(h => h.Patients)
            .WithMany(p => p.Hospitals)
            .UsingEntity(hp => hp.ToTable("PatientsHospitals"));

            //1:N between Hospital and Records
            modelBuilder.Entity<Hospital>()
            .HasMany(r => r.Records)
            .WithOne(h => h.Hospital)
            .HasForeignKey(h => h.HospitalId)
            .IsRequired();

            //1:N between Patient and Records
            modelBuilder.Entity<Patient>()
            .HasMany(r => r.Records)
            .WithOne(p => p.Patient)
            .HasForeignKey(p => p.PatientId)
            .IsRequired();
    
        }

        public DbSet<Hospital>? Hospitals { get; set; }
        public DbSet<Patient>? Patients { get; set; }
        public DbSet<Record>? Records { get; set; }
    }
}