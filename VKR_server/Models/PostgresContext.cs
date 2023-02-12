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

    public virtual DbSet<Challange> Challanges { get; set; }

    public virtual DbSet<ChallangeStudent> ChallangeStudents { get; set; }

    public virtual DbSet<ChallangeType> ChallangeTypes { get; set; }

    public virtual DbSet<PackCheck> PackChecks { get; set; }

    public virtual DbSet<Person> People { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<StudentAnswerCheck> StudentAnswerChecks { get; set; }

    public virtual DbSet<Task> Tasks { get; set; }

    public virtual DbSet<TasksPack> TasksPacks { get; set; }

    public virtual DbSet<TasksPackTask> TasksPackTasks { get; set; }

    public virtual DbSet<Teacher> Teachers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=200018");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresExtension("pg_catalog", "adminpack")
            .HasPostgresExtension("uuid-ossp");

        modelBuilder.Entity<Challange>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Challange_pkey");

            entity.ToTable("challange");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.ChallangeName)
                .HasColumnType("character varying")
                .HasColumnName("challange_name");
            entity.Property(e => e.ChallangeTarget).HasColumnName("challange_target");
            entity.Property(e => e.ChallangeType).HasColumnName("challange_type");

            entity.HasOne(d => d.ChallangeTypeNavigation).WithMany(p => p.Challanges)
                .HasForeignKey(d => d.ChallangeType)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("header_ch_type_fk");
        });

        modelBuilder.Entity<ChallangeStudent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("challange_student_pkey");

            entity.ToTable("challange_student");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.ChallangeId).HasColumnName("challange_id");
            entity.Property(e => e.StudentId).HasColumnName("student_id");

            entity.HasOne(d => d.Challange).WithMany(p => p.ChallangeStudents)
                .HasForeignKey(d => d.ChallangeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("challange_id_fk");

            entity.HasOne(d => d.Student).WithMany(p => p.ChallangeStudents)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("stud_id_fk");
        });

        modelBuilder.Entity<ChallangeType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("challange_type_pkey");

            entity.ToTable("challange_type");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.TypeName)
                .HasColumnType("character varying")
                .HasColumnName("type_name");
        });

        modelBuilder.Entity<PackCheck>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pack_check_pkey");

            entity.ToTable("pack_check");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CheckId).HasColumnName("check_id");
            entity.Property(e => e.PackId).HasColumnName("pack_id");

            entity.HasOne(d => d.Check).WithMany(p => p.PackChecks)
                .HasForeignKey(d => d.CheckId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("check_id_fk");

            entity.HasOne(d => d.Pack).WithMany(p => p.PackChecks)
                .HasForeignKey(d => d.PackId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pack_id_fk");
        });

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

        modelBuilder.Entity<StudentAnswerCheck>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("student_answer_check_pkey");

            entity.ToTable("student_answer_check");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.AnswerCorr).HasColumnName("answer_corr");
            entity.Property(e => e.StudentAnswer).HasColumnName("student_answer");
            entity.Property(e => e.StudentId).HasColumnName("student_id");
            entity.Property(e => e.TaskId).HasColumnName("task_id");

            entity.HasOne(d => d.Student).WithMany(p => p.StudentAnswerChecks)
                .HasForeignKey(d => d.StudentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("stud_id_fk");

            entity.HasOne(d => d.Task).WithMany(p => p.StudentAnswerChecks)
                .HasForeignKey(d => d.TaskId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("task_id_fk");
        });

        modelBuilder.Entity<Task>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("task_pkey");

            entity.ToTable("task");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.TaskAnswer).HasColumnName("task_answer");
            entity.Property(e => e.TaskText).HasColumnName("task_text");
        });

        modelBuilder.Entity<TasksPack>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tasks_pack_pkey");

            entity.ToTable("tasks_pack");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.TeacherId).HasColumnName("teacher_id");
            entity.Property(e => e.ThemeName).HasColumnName("theme_name");

            entity.HasOne(d => d.Teacher).WithMany(p => p.TasksPacks)
                .HasForeignKey(d => d.TeacherId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("teacher_id_fk");
        });

        modelBuilder.Entity<TasksPackTask>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tasks_pack_task_pkey");

            entity.ToTable("tasks_pack_task");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.PackId).HasColumnName("pack_id");
            entity.Property(e => e.TaskId).HasColumnName("task_id");

            entity.HasOne(d => d.Pack).WithMany(p => p.TasksPackTasks)
                .HasForeignKey(d => d.PackId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pack_id_fk");

            entity.HasOne(d => d.Task).WithMany(p => p.TasksPackTasks)
                .HasForeignKey(d => d.TaskId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("task_id_fk");
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
