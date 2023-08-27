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
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Services> Services { get; set; }
        public DbSet<Ambulance> Ambulances { get; set; }
        public DbSet<DoctorBookingPayment> DoctorBookingPayments { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<DoctorBookingPayment>().Property(p => p.CreateDate)
                .HasDefaultValueSql("getDate()");
            base.OnModelCreating(builder);

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer("Data Source=SQL8004.site4now.net;Initial Catalog=db_a9ddc8_hrbiproject8;User Id=db_a9ddc8_hrbiproject8_admin;Password=Sudu@123#");
            //optionsBuilder.UseSqlServer("Server=DESKTOP-QN2UA1B;Initial Catalog=Hrbi;User ID = sa; Password = 123456;Database=Hrbi;TrustServerCertificate=True");
            //optionsBuilder.UseSqlServer("Server=.;Initial Catalog=Hrbi;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True");

            base.OnConfiguring(optionsBuilder);
        }
    }
}
