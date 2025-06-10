using AuthorizationService.Constants;
using AuthorizationService.Domain.Commands;
using AuthorizationService.Domain.Responses;
using AuthorizationService.Services;
using Core.Domain.CommandHandlers;
using Core.Infrastructure.Results;
using Identity.Database.Entities;
using Identity.Database.Repositories;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AuthorizationService.Domain.CommandHandlers
{
    /// <summary>
    /// The command handler for registering the user
    /// </summary>
    public class RegisterUserCommandHandler : BaseCommandHandler<RegisterUserCommand, RegisterUserResponse>
    {
        private readonly UserManager<User> _userManager;
        private readonly IAuthorizationService _authorizationService;

        /// <summary>
        /// .ctor for RegisterUserCommandHandler
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="authorizationService"></param>
        /// <param name="logger"></param>
        public RegisterUserCommandHandler(
            UserManager<User> userManager,
            IAuthorizationService authorizationService,
            ILogger<RegisterUserCommandHandler> logger) : base(logger) 
        {
            _userManager = userManager;
            _authorizationService = authorizationService;
        }

        ///<inheritdoc/>
        protected override async Task<IApiResult<RegisterUserResponse>> HandleAsync(RegisterUserCommand command, CancellationToken ct)
        {
            var result = new ApiResult<RegisterUserResponse>();

            result.StatusCode = StatusCodes.Status201Created;

            var user = new User()
            {
                Id = Guid.NewGuid(),
                Email = command.Email,
                Name = command.Name,
                LastName = command.LastName,
                UserName = command.UserName,
                CompanyName = command.CompanyName,
                Code = command.Code,
            };

            var registerResult = await _authorizationService.RegisterUserAsync(user, command.Password, command.RoleCode, ct);
            if (registerResult.HasErrors)
            {
                result.MergeErrors(registerResult);
                result.StatusCode = StatusCodes.Status400BadRequest;
                result.Data.StatusCode = StatusCodes.Status400BadRequest;
            }

            result.Data = new() { Id = registerResult.Data, StatusCode = StatusCodes.Status201Created }; 

            return result;
        }

        ///<inheritdoc/>
        protected override async Task ValidateDataAsync(RegisterUserCommand request, CancellationToken ct)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);          
            if (existingUser != null)
            {
                throw new ValidationException("User with this email already exists.");
            }
            if (string.IsNullOrEmpty(request.Name))
            {
                throw new ValidationException("Name is empty.");
            }
            if (string.IsNullOrEmpty(request.LastName))
            {
                throw new ValidationException("Last name is empty.");
            }

            switch (request.RoleCode)
            {
                case "Student":
                    {
                        await ValidateStudentAsync(request, ct);
                        break;
                    }
                case "Teacher":
                    {
                        await ValidateTeacherAsync(request, ct);
                        break;
                    }
            }
        }

        /// <summary>
        /// Validates the student registration data
        /// </summary>
        /// <param name="request"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        private Task ValidateStudentAsync(RegisterUserCommand request, CancellationToken ct)
        {
            if (string.IsNullOrEmpty(request.UserName)) 
            {
                throw new ValidationException("Invalid username.");
            }
            if (!request.Email.Contains(AuthConstants.StudentMailPostfix))
            {
                throw new ValidationException("Invalid email.");
            }
            if (string.IsNullOrEmpty(request.Code))
            {
                throw new ValidationException("Invalid student code.");
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Validates the teacher registration data
        /// </summary>
        /// <param name="request"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        private Task ValidateTeacherAsync(RegisterUserCommand request, CancellationToken ct)
        {
            if (!request.Email.Contains(AuthConstants.TeacherMailPostfix))
            {
                throw new ValidationException("Invalid email.");
            }

            return Task.CompletedTask;
        }
    }
}
