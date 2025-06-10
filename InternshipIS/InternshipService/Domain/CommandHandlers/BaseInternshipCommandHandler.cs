using Core.Database.Entities;
using Core.Domain.CommandHandlers;
using Core.Domain.Requests;
using Core.Domain.Responses;
using Core.Exceptions;
using InternshipService.Database.Entities;
using InternshipService.Database.Repositories;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace InternshipService.Domain.CommandHandlers
{
    /// <summary>
    /// The base command handler for internships
    /// </summary>
    /// <typeparam name="TCommand"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public abstract class BaseInternshipCommandHandler<TCommand, TResponse> : BaseCommandHandler<TCommand, TResponse>
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
        protected BaseInternshipCommandHandler(
            IInternshipRepository internshipRepository,
            IInternshipTeacherRepository internshipTeacherRepository,
            IInternshipStudentRepository internshipStudentRepository,
            IInternshipCompanyRelativeRepository internshipCompanyRelativeRepository,
            IInternshipCategoryRepository internshipCategoryRepository,
            IInternshipLinkRepository internshipLinkRepository,
            ILogger logger) : base(logger)
        {
            InternshipRepository = internshipRepository;
            InternshipTeacherRepository = internshipTeacherRepository;
            InternshipStudentRepository = internshipStudentRepository;
            InternshipCompanyRelativeRepository = internshipCompanyRelativeRepository;
            InternshipCategoryRepository = internshipCategoryRepository;
            InternshipLinkRepository = internshipLinkRepository;
        }

        protected IInternshipRepository InternshipRepository { get; }
        protected IInternshipTeacherRepository InternshipTeacherRepository { get; }
        protected IInternshipStudentRepository InternshipStudentRepository { get; }
        protected IInternshipCompanyRelativeRepository InternshipCompanyRelativeRepository { get; }
        protected IInternshipCategoryRepository InternshipCategoryRepository { get; }
        protected IInternshipLinkRepository InternshipLinkRepository { get; }

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
        protected IQueryable<T> ApplyOrdering<T>(IQueryable<T> query, string propertyName, int orderDirection)
            where T : class
        {
            if(propertyName.Equals("null") || string.IsNullOrEmpty(propertyName))
            {
                return query;
            }

            try
            {
                var entityType = typeof(T);

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

                return (IQueryable<T>)method.Invoke(null, new object[] { query, orderByExpression });
            }
            catch (Exception ex) 
            {
                Logger.LogError("Unable to apply ordering! {0}, {1}, {2}.", query?.ToString(), propertyName, orderDirection);
                throw new ProcessingException("Unable to apply ordering!");
            }
        }

        /// <summary>
        /// Applies the filtering
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ProcessingException"></exception>
        protected IQueryable<T> ApplyFilter<T>(IQueryable<T> query, string propertyName, object value)
            where T : class
        {
            if (propertyName.Equals("null") || string.IsNullOrEmpty(propertyName))
            {
                return query;
            }

            try
            {
                var entityType = typeof(T);

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

                var convertedValue = Convert.ChangeType(value, property.PropertyType);
                var constant = Expression.Constant(convertedValue);

                var condition = Expression.Equal(propertyAccess, constant);
                var lambda = Expression.Lambda<Func<T, bool>>(condition, parameter);

                return query.Where(lambda);
            }
            catch (Exception ex)
            {
                Logger.LogError("Unable to apply filtering! {0}, {1}.", query?.ToString(), propertyName);
                Logger.LogError(ex.Message);
                throw new ProcessingException("Unable to apply filtering!");
            }
        }

        /// <summary>
        /// Applies the created by me filter
        /// </summary>
        /// <param name="query"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        protected IQueryable<T> ApplyCreatedByMeFilter<T>(IQueryable<T> query, string email)
            where T : class
        {
            return ApplyFilter<T>(query, "CreationAuthor", email);
        }

        /// <summary>
        /// Gets the student by their id and adds them into the internship
        /// </summary>
        /// <param name="internship"></param>
        /// <param name="studentId"></param>
        /// <param name="ct"></param>
        /// <returns>True if a student was added to the internship, otherwise false</returns>
        protected async Task<bool> AddStudentsAsync(Internship internship, Guid? studentId, CancellationToken ct)
        {
            if (studentId != null && studentId != Guid.Empty)
            {
                var studentResult = await InternshipStudentRepository.GetAsync(studentId.Value, ct);
                if (studentResult.HasErrors)
                {
                    Logger.LogError(studentResult.GetErrorMessage());
                    return false;
                }

                if (studentResult.Data == null)
                {
                    Logger.LogError("The requested student is not available.");
                    throw new KeyNotFoundException("The requested student is not available");
                }

                if(internship.Students.Select(x=>x.Id).ToList().Contains(studentResult.Data.Id))
                {
                    return false;
                }
                else
                {
                    internship.Students.Add(studentResult.Data);
                    return true;
                }
            }
            else
            {
                internship.Students.Clear();
                return false;
            }
        }

        /// <summary>
        /// Gets the teachers by their ids and adds them into the internship
        /// </summary>
        /// <param name="internship"></param>
        /// <param name="teacherIds"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        protected async Task AddTeachersAsync(Internship internship, List<Guid>? teacherIds, CancellationToken ct)
        {
            if (teacherIds != null && teacherIds.Any())
            {
                internship.Teachers.Clear();

                foreach (var teacherId in teacherIds)
                {
                    if (teacherId != Guid.Empty)
                    {
                        var teacherResult = await InternshipTeacherRepository.GetAsync(teacherId, ct);
                        if (teacherResult.HasErrors)
                        {
                            Logger.LogError(teacherResult.GetErrorMessage());
                            continue;
                        }

                        internship.Teachers.Add(teacherResult.Data);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the company relatives by their ids and adds them into the internship
        /// </summary>
        /// <param name="internship"></param>
        /// <param name="relativeIds"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        protected async Task AddCompanyRelativesAsync(Internship internship, List<Guid>? relativeIds, CancellationToken ct)
        {
            if (relativeIds != null && relativeIds.Any())
            {
                internship.CompanyRelatives.Clear();

                foreach (var relativeId in relativeIds)
                {
                    if (relativeId != Guid.Empty)
                    {
                        var relativeResult = await InternshipCompanyRelativeRepository.GetAsync(relativeId, ct);
                        if (relativeResult.HasErrors)
                        {
                            Logger.LogError(relativeResult.GetErrorMessage());
                            continue;
                        }

                        internship.CompanyRelatives.Add(relativeResult.Data);
                    }
                }
            }
        }

        /// <summary>
        /// Adds the selected category to the internship
        /// </summary>
        /// <param name="internship"></param>
        /// <param name="categoryId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        protected async Task AddCategoryAsync(Internship internship, Guid? categoryId, CancellationToken ct)
        {
            if (categoryId != null && categoryId != Guid.Empty)
            {
                var categoryResult = await InternshipCategoryRepository.GetAsync(categoryId.Value, ct);
                if (categoryResult.HasErrors)
                {
                    Logger.LogError(categoryResult.GetErrorMessage());
                    return;
                }

                if (categoryResult.Data == null)
                {
                    Logger.LogError("The requested category is not available.");
                    throw new KeyNotFoundException("The requested category is not available");
                }

                internship.InternshipCategory = categoryResult.Data;
            }
        }
    }
}
