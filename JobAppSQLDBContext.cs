using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using JobApplication.Models;

#nullable disable

namespace JobApplication
{
    public partial class JobAppSQLDBContext : DbContext
    {
        public JobAppSQLDBContext()
        {
        }

        public JobAppSQLDBContext(DbContextOptions<JobAppSQLDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AnswersType> AnswersTypes { get; set; }
        public virtual DbSet<Application> Applications { get; set; }
        public virtual DbSet<ApplicationAnswer> ApplicationAnswers { get; set; }
        public virtual DbSet<Question> Questions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=tcp:jobappsqlserver.database.windows.net,1433;Initial Catalog=JobAppSQLDB;Persist Security Info=False;User ID=mbiasuzzi;Password=Viadellecave1b$;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<AnswersType>(entity =>
            {
                entity.HasKey(e => e.AnswerId)
                    .HasName("PK__answers___3372431896A13E49");

                entity.ToTable("answers_type");

                entity.Property(e => e.AnswerId).HasColumnName("answer_id");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.QuestionId).HasColumnName("question_id");

                entity.Property(e => e.ValidAnswer)
                    .HasMaxLength(1000)
                    .IsUnicode(false)
                    .HasColumnName("valid_answer");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.AnswersTypes)
                    .HasForeignKey(d => d.QuestionId)
                    .HasConstraintName("FK__answers_t__quest__5EBF139D");
            });

            modelBuilder.Entity<Application>(entity =>
            {
                entity.ToTable("applications");

                entity.Property(e => e.ApplicationId).HasColumnName("application_id");

                entity.Property(e => e.Valid).HasColumnName("valid");
            });

            modelBuilder.Entity<ApplicationAnswer>(entity =>
            {
                entity.ToTable("application_answers");

                entity.Property(e => e.ApplicationAnswerId).HasColumnName("application_answer_id");

                entity.Property(e => e.Answer)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("answer");

                entity.Property(e => e.ApplicationId).HasColumnName("application_id");

                entity.Property(e => e.QuestionId).HasColumnName("question_id");

                entity.HasOne(d => d.Application)
                    .WithMany(p => p.ApplicationAnswers)
                    .HasForeignKey(d => d.ApplicationId)
                    .HasConstraintName("FK__applicati__appli__71D1E811");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.ApplicationAnswers)
                    .HasForeignKey(d => d.QuestionId)
                    .HasConstraintName("FK__applicati__quest__72C60C4A");
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.ToTable("questions");

                entity.Property(e => e.QuestionId).HasColumnName("question_id");

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.QuestionText)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("question_text");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
