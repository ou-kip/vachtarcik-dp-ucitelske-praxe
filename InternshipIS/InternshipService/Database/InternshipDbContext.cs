using InternshipService.Database.Entities;
using InternshipService.Database.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace InternshipService.Database
{
    /// <summary>
    /// The internship db context
    /// </summary>
    public class InternshipDbContext : DbContext
    {
        private readonly IInternshipCompanyRelativeEntityConfiguration _companyRelativeEntityConfiguration;
        private readonly IInternshipEntityConfiguration _entityConfiguration;
        private readonly IInternshipStudentEntityConfiguration _studentEntityConfiguration;
        private readonly IInternshipTaskEntityConfiguration _taskEntityConfiguration;
        private readonly IInternshipTaskFileEntityConfiguration _taskFileEntityConfiguration;
        private readonly IInternshipTeacherEntityConfiguration _teacherEntityConfiguration;
        private readonly IInternshipLinkEntityConfiguration _internshipLinkEntityConfiguration;
        private readonly IInternshipCategoryEntityConfiguration _internshipCategoryEntityConfiguration;
        private readonly IInternshipTaskLinkEntityConfiguration _internshipTaskLinkEntityConfiguration;
        private readonly IInternshipTaskSolutionEntityConfiguration _internshipTaskSolutionEntityConfiguration;
        private readonly IInternshipTaskSolutionFileEntityConfiguration _internshipTaskSolutionFileEntityConfiguration;

        /// <summary>
        /// default .ctor
        /// </summary>
        public InternshipDbContext()
        {
        }

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="companyRelativeEntityConfiguration"></param>
        /// <param name="entityConfiguration"></param>
        /// <param name="studentEntityConfiguration"></param>
        /// <param name="taskEntityConfiguration"></param>
        /// <param name="taskFileEntityConfiguration"></param>
        /// <param name="teacherEntityConfiguration"></param>
        /// <param name="internshipLinkEntityConfiguration"></param>
        /// <param name="internshipTaskSolutionEntityConfiguration"></param>
        public InternshipDbContext(
            IInternshipCompanyRelativeEntityConfiguration companyRelativeEntityConfiguration,
            IInternshipEntityConfiguration entityConfiguration,
            IInternshipStudentEntityConfiguration studentEntityConfiguration, 
            IInternshipTaskEntityConfiguration taskEntityConfiguration, 
            IInternshipTaskFileEntityConfiguration taskFileEntityConfiguration, 
            IInternshipTeacherEntityConfiguration teacherEntityConfiguration,
            IInternshipLinkEntityConfiguration internshipLinkEntityConfiguration,
            IInternshipCategoryEntityConfiguration internshipCategoryEntityConfiguration,
            IInternshipTaskLinkEntityConfiguration internshipTaskLinkEntityConfiguration,
            IInternshipTaskSolutionEntityConfiguration internshipTaskSolutionEntityConfiguration,
            IInternshipTaskSolutionFileEntityConfiguration internshipTaskSolutionFileEntityConfiguration,
            DbContextOptions options) : base(options)
        {
            _companyRelativeEntityConfiguration = companyRelativeEntityConfiguration;
            _entityConfiguration = entityConfiguration;
            _studentEntityConfiguration = studentEntityConfiguration;
            _taskEntityConfiguration = taskEntityConfiguration;
            _taskFileEntityConfiguration = taskFileEntityConfiguration;
            _teacherEntityConfiguration = teacherEntityConfiguration;
            _internshipLinkEntityConfiguration = internshipLinkEntityConfiguration;
            _internshipCategoryEntityConfiguration = internshipCategoryEntityConfiguration;
            _internshipTaskLinkEntityConfiguration = internshipTaskLinkEntityConfiguration;
            _internshipTaskSolutionEntityConfiguration = internshipTaskSolutionEntityConfiguration;
            _internshipTaskSolutionFileEntityConfiguration = internshipTaskSolutionFileEntityConfiguration;
        }

        public DbSet<Internship> Internships { get; set; }
        public DbSet<InternshipStudent> InternshipStudents { get; set; }
        public DbSet<InternshipTeacher> InternshipTeachers { get; set; }
        public DbSet<InternshipCompanyRelative> InternshipCompanyRelatives { get; set; }
        public DbSet<InternshipTask> InternshipTasks { get; set; }
        public DbSet<InternshipTaskFile> InternshipTaskFiles { get; set; }
        public DbSet<InternshipLink> InternshipLinks { get; set; }
        public DbSet<InternshipCategory> InternshipCategories { get; set; }
        public DbSet<InternshipTaskLink> InternshipTaskLinks { get; set; }
        public DbSet<InternshipTaskSolution> InternshipTaskSolutions { get; set; }
        public DbSet<InternshipTaskSolution> InternshipTaskSolutionFiles { get; set; }

        ///<inheritdoc/>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            _companyRelativeEntityConfiguration.Configure(modelBuilder.Entity<InternshipCompanyRelative>());
            _entityConfiguration.Configure(modelBuilder.Entity<Internship>());
            _studentEntityConfiguration.Configure(modelBuilder.Entity<InternshipStudent>());
            _taskEntityConfiguration.Configure(modelBuilder.Entity<InternshipTask>());
            _taskFileEntityConfiguration.Configure(modelBuilder.Entity<InternshipTaskFile>());
            _teacherEntityConfiguration.Configure(modelBuilder.Entity<InternshipTeacher>());
            _internshipLinkEntityConfiguration.Configure(modelBuilder.Entity<InternshipLink>());
            _internshipCategoryEntityConfiguration.Configure(modelBuilder.Entity<InternshipCategory>());
            _internshipTaskLinkEntityConfiguration.Configure(modelBuilder.Entity<InternshipTaskLink>());
            _internshipTaskSolutionEntityConfiguration.Configure(modelBuilder.Entity<InternshipTaskSolution>());
            _internshipTaskSolutionFileEntityConfiguration.Configure(modelBuilder.Entity<InternshipTaskSolutionFile>());

            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("intern");
        }
    }
}
