using Microsoft.EntityFrameworkCore;
using retonet.Models; // Asegúrate de usar el namespace correcto para tus modelos.

public class RetonetDbContext : DbContext
{
    public RetonetDbContext(DbContextOptions<RetonetDbContext> options)
        : base(options)
    {
    }

    public DbSet<User>? Users { get; set; }
    // Agrega DbSet para otras entidades según sea necesario.
}