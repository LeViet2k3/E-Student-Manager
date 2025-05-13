using Microsoft.EntityFrameworkCore;

namespace StudentApp.Models
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseClass> CourseClasses { get; set; }
        public DbSet<CourseRegistration> CourseRegistrations { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<StudyPlan> StudyPlans { get; set; }
        public DbSet<TeachingSchedule> TeachingSchedules { get; set; }
        public DbSet<TrainingProgram> TrainingPrograms { get; set; }
        public DbSet<Question> Questions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<CourseRegistration>()
        .HasKey(cr => new { cr.MaSV, cr.MaLHP });

        modelBuilder.Entity<CourseClass>()
            .HasOne(cc => cc.Course)
            .WithMany(c => c.CourseClasses)
            .HasForeignKey(cc => cc.MaHP)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CourseClass>()
            .HasOne(cc => cc.Teachers)
            .WithMany(t => t.CourseClasses)
            .HasForeignKey(cc => cc.MaGV)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CourseRegistration>()
            .HasOne(cr => cr.Students)
            .WithMany(s => s.CourseRegistrations)
            .HasForeignKey(cr => cr.MaSV)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CourseRegistration>()
            .HasOne(cr => cr.CourseClasses)
            .WithMany(cc => cc.CourseRegistrations)
            .HasForeignKey(cr => cr.MaLHP)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Grade>()
            .HasOne(g => g.Students)
            .WithMany(s => s.Grades)
            .HasForeignKey(g => g.MaSV)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Grade>()
            .HasOne(g => g.CourseClasses)
            .WithMany(cc => cc.Grades)
            .HasForeignKey(g => g.MaLHP)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Schedule>()
            .HasOne(sch => sch.Students)
            .WithMany(s => s.Schedules)
            .HasForeignKey(sch => sch.MaSV)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Schedule>()
            .HasOne(sch => sch.CourseClasses)
            .WithMany(cc => cc.Schedules)
            .HasForeignKey(sch => sch.MaLHP)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<StudyPlan>()
            .HasOne(sp => sp.Courses)
            .WithMany(c => c.StudyPlans)
            .HasForeignKey(sp => sp.MaHP)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<TeachingSchedule>()
            .HasOne(ts => ts.Teachers)
            .WithMany(t => t.TeachingSchedules)
            .HasForeignKey(ts => ts.MaGV)
            .OnDelete(DeleteBehavior.Restrict); // Quan trọng

        modelBuilder.Entity<TeachingSchedule>()
            .HasOne(ts => ts.CourseClasses)
            .WithMany(cc => cc.TeachingSchedules)
            .HasForeignKey(ts => ts.MaLHP)
            .OnDelete(DeleteBehavior.Restrict); // Thêm dòng này để tránh lỗi cascade

        modelBuilder.Entity<TrainingProgram>()
            .HasOne(tp => tp.Courses)
            .WithMany(c => c.TrainingPrograms)
            .HasForeignKey(tp => tp.MaHP)
            .OnDelete(DeleteBehavior.Restrict);
    }

    }
}
