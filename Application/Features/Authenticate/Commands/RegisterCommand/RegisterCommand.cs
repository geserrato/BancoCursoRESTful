using Application.DTOs.Users;
using Application.Interfaces;
using Application.Wrappers;
using MediatR;

namespace Application.Features.Authenticate.Commands.RegisterCommand;

public class RegisterCommand : IRequest<Response<string>>
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
    public string Origin { get; set; }

    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Response<string>>
    {
        private readonly IAccountService? _accountService;

        public RegisterCommandHandler(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<Response<string>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            return await _accountService.RegisterAsync(new RegisterRequest
            {
                Email = request.Email,
                Password = request.Password,
                ConfirmPassword = request.ConfirmPassword,
                UserName = request.UserName,
                Name = request.Name,
                Surname = request.Surname
            }, request.Origin);
        }
    }
}