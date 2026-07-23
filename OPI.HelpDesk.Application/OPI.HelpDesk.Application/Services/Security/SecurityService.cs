using OPI.HelpDesk.Application.Dtos.Security;
using OPI.HelpDesk.Application.Interfaces.Security;
using OPI.HelpDesk.Application.Interfaces.UnitOfWorks;
using OPI.HelpDesk.Application.Specifications;
using OPI.HelpDessk.Domain.Entities;
using OPI.HelpDessk.Domain.Entities.Enums;
using System.Security.Authentication;

namespace OPI.HelpDesk.Application.Services.Security
{
    public class SecurityService(IUnitOfWork uow, IAuthService authService) : ISecurityService
    {
        public async Task<AuthResponseDto> Login(LoginRequestDto request, CancellationToken ct)
        {
            SpecificationBuilder<User> spec = new SpecificationBuilder<User>()
                .Where(u => u.Email == request.Email);

            User user = await uow.Repository<User>().GetAsync(spec, ct);
            uow.Dispose();
            if (user == null || !authService.VerifyPassword(request.Password, user.HashPassword))
                throw new AuthenticationException("Credenciales incorrectas");
            if (!user.Enable)
                throw new AuthenticationException("El usuario se encuentra desactivado");

            var token = authService.GenerateJwtToken(user);
            //TODO:Falta refresh token
            return new AuthResponseDto(
                token,
                string.Empty,
                DateTime.UtcNow.AddMinutes(15)
                );

        }

        public void Logout()
        {
            throw new NotImplementedException();
        }

        public async Task<AuthResponseDto> SingUp(SingUpRequestDto request, CancellationToken ct)
        {
            SpecificationBuilder<User> spec = new SpecificationBuilder<User>()
                .Where(u => u.Email == request.Email);

            if (await uow.Repository<User>().GetAsync(spec, ct) is not null)
                throw new Exception("El correo ya esta registrado.");

            string passwordHash = authService.HashPassword(request.Password);
            Guid newUserId = Guid.NewGuid();

            User newUser = new User
            {
                Id = newUserId,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                HashPassword = passwordHash,
                Role = RolesEnum.Client
            };

             await uow.Repository<User>().AddAsync(newUser, ct);
             await uow.SaveChangesAsync();
            uow.Dispose();

            var token = authService.GenerateJwtToken(newUser);
            //TODO:Falta refresh token
            return new AuthResponseDto(
                token,
                string.Empty,
                DateTime.UtcNow.AddMinutes(15)
             );
        }
    }
}
