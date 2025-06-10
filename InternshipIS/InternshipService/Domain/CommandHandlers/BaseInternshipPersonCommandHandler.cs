using Core.Domain.CommandHandlers;
using Core.Domain.Requests;
using Core.Domain.Responses;
using Core.Exceptions;
using InternshipService.Database.Entities;
using InternshipService.Database.Repositories;
using System.Linq.Expressions;

namespace InternshipService.Domain.CommandHandlers
{
    /// <summary>
    /// The base command handler for internships
    /// </summary>
    /// <typeparam name="TCommand"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public abstract class BaseInternshipPersonCommandHandler<TCommand, TResponse> : BaseCommandHandler<TCommand, TResponse>
        where TCommand : IBaseRequest<TResponse>
        where TResponse : IBaseResponse
    {
        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="internshipRepository"></param>
        /// <param name="internshipTeacherRepository"></param>
        /// <param name="internshipStudentRepository"></param>
        /// <param name="internshipCompanyRelativeRepository"></param>
        /// <param name="internshipCategoryRepository"></param>
        /// <param name="internshipLinkRepository"></param>
        /// <param name="logger"></param>
        protected BaseInternshipPersonCommandHandler(
            IInternshipRepository internshipRepository,
            IInternshipTeacherRepository internshipTeacherRepository,
            IInternshipStudentRepository internshipStudentRepository,
            IInternshipCompanyRelativeRepository internshipCompanyRelativeRepository,
            ILogger logger) : base(logger)
        {
            InternshipRepository = internshipRepository;
            InternshipTeacherRepository = internshipTeacherRepository;
            InternshipStudentRepository = internshipStudentRepository;
            InternshipCompanyRelativeRepository = internshipCompanyRelativeRepository;
        }

        protected IInternshipRepository InternshipRepository { get; }
        protected IInternshipTeacherRepository InternshipTeacherRepository { get; }
        protected IInternshipStudentRepository InternshipStudentRepository { get; }
        protected IInternshipCompanyRelativeRepository InternshipCompanyRelativeRepository { get; }

        /// <summary>
        /// Gets the name of the student by student id
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        protected async Task<string> GetStudentNameAsync(Guid? studentId, CancellationToken ct)
        {
            if (studentId != null && studentId != Guid.Empty)
            {
                var studentResult = await InternshipStudentRepository.GetAsync((Guid)studentId, ct);
                return studentResult.Data != null ? string.Join(" ", studentResult.Data.Name, studentResult.Data.LastName) : string.Empty;
            }

            return string.Empty;
        }

        /// <summary>
        /// Gets the name of the student by student id
        /// </summary>
        /// <param name="studentEmail"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        protected async Task<string> GetStudentNameAsync(string studentEmail, CancellationToken ct)
        {
            if (!string.IsNullOrEmpty(studentEmail) && !string.IsNullOrWhiteSpace(studentEmail))
            {
                var studentResult = await InternshipStudentRepository.GetByEmailAsync(studentEmail, ct);
                return studentResult.Data != null ? string.Join(" ", studentResult.Data.Name, studentResult.Data.LastName) : string.Empty;
            }

            return string.Empty;
        }

        /// <summary>
        /// Gets the name of the teacher by teacher id
        /// </summary>
        /// <param name="teacherId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        protected async Task<string> GetTeacherNameAsync(Guid? teacherId, CancellationToken ct)
        {
            if (teacherId != null && teacherId != Guid.Empty)
            {
                var teacherResult = await InternshipTeacherRepository.GetAsync((Guid)teacherId, ct);
                return teacherResult.Data != null ? string.Join(" ", teacherResult.Data.Name, teacherResult.Data.LastName) : string.Empty;
            }

            return string.Empty;
        }

        /// <summary>
        /// Gets the name of the teacher by email
        /// </summary>
        /// <param name="teacherEmail"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        protected async Task<string> GetTeacherNameAsync(string teacherEmail, CancellationToken ct)
        {
            if (!string.IsNullOrEmpty(teacherEmail) && !string.IsNullOrWhiteSpace(teacherEmail))
            {
                var teacherResult = await InternshipTeacherRepository.GetByEmailAsync(teacherEmail, ct);
                return teacherResult != null ? string.Join(" ", teacherResult.Name, teacherResult.LastName) : string.Empty;
            }

            return string.Empty;
        }

        /// <summary>
        /// Gets the name of the company relative by id
        /// </summary>
        /// <param name="companyRelativeId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        protected async Task<string> GetCompanyRelativeNameAsync(Guid? companyRelativeId, CancellationToken ct)
        {
            if (companyRelativeId != null && companyRelativeId != Guid.Empty)
            {
                var relativeResult = await InternshipCompanyRelativeRepository.GetAsync((Guid)companyRelativeId, ct);
                return relativeResult.Data != null ? string.Join(" ", relativeResult.Data.Name, relativeResult.Data.LastName) : string.Empty;
            }

            return string.Empty;
        }

        /// <summary>
        /// Gets the name of the company relative by email
        /// </summary>
        /// <param name="companyRelativeEmail"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        protected async Task<string> GetCompanyRelativeNameAsync(string companyRelativeEmail, CancellationToken ct)
        {
            if (!string.IsNullOrEmpty(companyRelativeEmail) && !string.IsNullOrWhiteSpace(companyRelativeEmail))
            {
                var relativeResult = await InternshipCompanyRelativeRepository.GetByEmailAsync(companyRelativeEmail, ct);
                return relativeResult != null ? string.Join(" ", relativeResult.Name, relativeResult.LastName) : string.Empty;
            }

            return string.Empty;
        }

        /// <summary>
        /// Gets the name of the author by email
        /// </summary>
        /// <param name="email"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        protected async Task<string> GetAuthorNameAsync(string email, CancellationToken ct)
        {
            if (email.Contains("@osu"))
            {
                var name = await GetStudentNameAsync(email, ct);
                if (string.IsNullOrEmpty(name))
                {
                    name = await GetTeacherNameAsync(email, ct);
                }

                return name;
            }
            else
            {
                return await GetCompanyRelativeNameAsync(email, ct);
            }
        }

        /// <summary>
        /// Applies the ordering
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="propertyName"></param>
        /// <param name="orderDirection">0 - ascending, 1 - descending</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ProcessingException"></exception>
        protected IQueryable<Internship> ApplyOrdering(IQueryable<Internship> query, string propertyName, int orderDirection)
        {
            if (propertyName.Equals("null") || string.IsNullOrEmpty(propertyName))
            {
                return query;
            }

            try
            {
                var entityType = typeof(Internship);

                var property = entityType
                    .GetProperties()
                    .FirstOrDefault(p => string.Equals(p.Name, propertyName, StringComparison.OrdinalIgnoreCase));

                if (property == null)
                {
                    Logger.LogError($"Property '{propertyName}' does not exist on type '{entityType.Name}'.");
                    throw new ArgumentException($"Property '{propertyName}' does not exist on type '{entityType.Name}'.");
                }

                var parameter = Expression.Parameter(entityType, "x");
                var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                var orderByExpression = Expression.Lambda(propertyAccess, parameter);
                var methodName = orderDirection == 0 ? "OrderBy" : "OrderByDescending";

                var method = typeof(Queryable)
                    .GetMethods()
                    .First(m => m.Name == methodName && m.GetParameters().Length == 2)
                    .MakeGenericMethod(entityType, property.PropertyType);

                return (IQueryable<Internship>)method.Invoke(null, new object[] { query, orderByExpression });
            }
            catch (Exception ex)
            {
                Logger.LogError("Unable to apply ordering! {0}, {1}, {2}.", query?.ToString(), propertyName, orderDirection);
                throw new ProcessingException("Unable to apply ordering!");
            }
        }

        /// <summary>
        /// Applies the created by me filter
        /// </summary>
        /// <param name="query"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        protected static IQueryable<Internship> ApplyCreatedByMeFilter(IQueryable<Internship> query, string email)
        {
            return query.Where(x => x.CreationAuthor == email);
        }
    }
}
