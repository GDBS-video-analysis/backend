using Microsoft.EntityFrameworkCore;
using Web.Entities;

namespace Web.DataBaseContext
{
    public class VideoAnalisysDBContext(DbContextOptions<VideoAnalisysDBContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EmployeeMarksEvents> EmployeeMarks { get; set; }
        public DbSet<UnregisterPersonMarksEvents> UnregisterPersonMarks { get; set; }
        public DbSet<MinioFile> Files { get; set; }
        public DbSet<VideoAnalisysStatus> VideoAnalisysStatuses { get; set; }
        
    }
}
