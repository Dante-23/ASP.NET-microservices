using Microsoft.EntityFrameworkCore;

namespace TodoService.Models;

public class TodoDbContext : DbContext
{
    public TodoDbContext() {}
    public TodoDbContext(DbContextOptions<TodoDbContext> options)
        : base(options)
    {
    }

    public DbSet<Todo> Todos { get; set; }
}