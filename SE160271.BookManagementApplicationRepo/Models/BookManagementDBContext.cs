using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace SE160271.BookManagementApplicationRepo.Models
{
    public partial class BookManagementDBContext : DbContext
    {
        public BookManagementDBContext()
        {
        }

        public BookManagementDBContext(DbContextOptions<BookManagementDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Address> Addresses { get; set; } = null!;
        public virtual DbSet<Book> Books { get; set; } = null!;
        public virtual DbSet<Press> Presses { get; set; } = null!;

        /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server= DESKTOP-JMQJTOM\\CAOKIENQUOC;uid=sa;pwd=12345;database=BookManagementDB;TrustServerCertificate=True;");
            }
        }*/

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(GetConnectionString());
            }
        }
        private string GetConnectionString()
        {
            IConfiguration config = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", true, true)
                        .Build();
            var strConn = config.GetConnectionString("BookDB");

            return strConn;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>(entity =>
            {
                entity.Property(e => e.City).HasMaxLength(100);

                entity.Property(e => e.Country).HasMaxLength(100);
            });

            modelBuilder.Entity<Book>(entity =>
            {
                entity.Property(e => e.Author).HasMaxLength(100);

                entity.Property(e => e.Isbn)
                    .HasMaxLength(20)
                    .HasColumnName("ISBN");

                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Title).HasMaxLength(200);

                entity.HasOne(d => d.Press)
                    .WithMany(p => p.Books)
                    .HasForeignKey(d => d.PressId)
                    .HasConstraintName("FK__Books__PressId__3B75D760");
            });

            modelBuilder.Entity<Press>(entity =>
            {
                entity.Property(e => e.Category).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.HasOne(d => d.Address)
                    .WithMany(p => p.Presses)
                    .HasForeignKey(d => d.AddressId)
                    .HasConstraintName("FK__Presses__Address__38996AB5");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
