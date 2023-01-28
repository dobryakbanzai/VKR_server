using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace VKR_server.Models;

public partial class PostgresContext : DbContext
{
    public PostgresContext()
    {
    }

    public PostgresContext(DbContextOptions<PostgresContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Person> People { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<Teacher> Teachers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=200018");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresExtension("pg_catalog", "adminpack")
            .HasPostgresExtension("uuid-ossp");

        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("person_pkey");

            entity.ToTable("person");

            entity.HasIndex(e => e.Id, "person_id_key").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Firstname).HasColumnName("firstname");
            entity.Property(e => e.Lastname).HasColumnName("lastname");
            entity.Property(e => e.PersonRole).HasColumnName("person_role");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("students_pkey");

            entity.ToTable("students");

            entity.HasIndex(e => e.Id, "students_id_key").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.AryProg)
                .HasDefaultValueSql("0")
                .HasColumnName("ary_prog");
            entity.Property(e => e.DerProg)
                .HasDefaultValueSql("0")
                .HasColumnName("der_prog");
            entity.Property(e => e.EdClass).HasColumnName("ed_class");
            entity.Property(e => e.Firstname).HasColumnName("firstname");
            entity.Property(e => e.Lastname).HasColumnName("lastname");
            entity.Property(e => e.Login).HasColumnName("login");
            entity.Property(e => e.Pass).HasColumnName("pass");
            entity.Property(e => e.PersonRole)
                .HasDefaultValueSql("'student'::text")
                .HasColumnName("person_role");
            entity.Property(e => e.TasksProg)
                .HasDefaultValueSql("0")
                .HasColumnName("tasks_prog");
            entity.Property(e => e.TeacherId).HasColumnName("teacher_id");

            entity.HasOne(d => d.Teacher).WithMany(p => p.Students)
                .HasForeignKey(d => d.TeacherId)
                .HasConstraintName("students_teacher_id_fkey");
        });

        modelBuilder.Entity<Teacher>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("teachers_pkey");

            entity.ToTable("teachers");

            entity.HasIndex(e => e.Id, "teachers_id_key").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Experience).HasColumnName("experience");
            entity.Property(e => e.Firstname).HasColumnName("firstname");
            entity.Property(e => e.Lastname).HasColumnName("lastname");
            entity.Property(e => e.Login).HasColumnName("login");
            entity.Property(e => e.Pass).HasColumnName("pass");
            entity.Property(e => e.PersonRole)
                .HasDefaultValueSql("'teacher'::text")
                .HasColumnName("person_role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
