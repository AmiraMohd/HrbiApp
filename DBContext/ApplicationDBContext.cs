using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBContext
{
    public class ApplicationDBContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDBContext()
        {
            
        }
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }

        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Specialization> Specializations { get; set; }
        public DbSet<NurseService> NurseServices { get; set; }
        public DbSet<NurseBooking> NurseBookings { get; set; }
        public DbSet<LabService> LabServices { get; set; }
        public DbSet<LabServiceBooking> LabServiceBookings { get; set; }
        public DbSet<DoctorBooking> DoctorBookings { get; set; }
        public DbSet<DoctorPosition> DoctorPositions { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<ExceptionLog> ExceptionLogs { get; set; }
        public DbSet<OTP> OTPs { get; set; }
        public DbSet<ApplicationVersion> ApplicationVersions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer("Server=192.119.80.230;Initial Catalog=TzwedTest;User ID = 21techis; Password = techis@123#;Database=TzwedTest;Trusted_Connection=False;MultipleActiveResultSets=true");
            optionsBuilder.UseSqlServer("Server=DESKTOP-QN2UA1B;Initial Catalog=Hrbi;User ID = sa; Password = 123456;Database=Hrbi;TrustServerCertificate=True");

            base.OnConfiguring(optionsBuilder);
        }
    }
}
