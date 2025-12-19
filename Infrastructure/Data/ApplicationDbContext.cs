using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Questionario> Questionarios { get; set; }
    public DbSet<Pergunta> Perguntas { get; set; }
    public DbSet<Alternativa> Alternativas { get; set; }
    public DbSet<Resposta> Respostas { get; set; }
    public DbSet<ResultadoSumarizado> ResultadosSumarizados { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Questionario>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Titulo).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Descricao).HasMaxLength(1000);
            entity.HasMany(e => e.Perguntas)
                .WithOne(e => e.Questionario)
                .HasForeignKey(e => e.QuestionarioId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Pergunta>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Texto).IsRequired().HasMaxLength(500);
            entity.HasMany(e => e.Alternativas)
                .WithOne(e => e.Pergunta)
                .HasForeignKey(e => e.PerguntaId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(e => e.Respostas)
                .WithOne(e => e.Pergunta)
                .HasForeignKey(e => e.PerguntaId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Alternativa>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Texto).IsRequired().HasMaxLength(300);
        });

        modelBuilder.Entity<Resposta>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.TextoResposta).HasMaxLength(1000);
            entity.Property(e => e.IpAddress).HasMaxLength(50);
            entity.HasOne(e => e.Alternativa)
                .WithMany()
                .HasForeignKey(e => e.AlternativaId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ResultadoSumarizado>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Percentual).HasPrecision(5, 2);
            entity.HasOne(e => e.Pergunta)
                .WithMany()
                .HasForeignKey(e => e.PerguntaId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.Alternativa)
                .WithMany()
                .HasForeignKey(e => e.AlternativaId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
