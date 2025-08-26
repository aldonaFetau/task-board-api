using Microsoft.EntityFrameworkCore;
using TaskBoardAPI.Models;

namespace TaskBoardAPI.Data
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

		public DbSet<TasksList> TasksList { get; set; }
		public DbSet<TaskItem> Tasks { get; set; }
      

    }
}
