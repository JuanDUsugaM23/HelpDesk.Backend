using OPI.HelpDesk.Application.Dtos.Shares;
using OPI.HelpDesk.Application.Dtos.Users;
using OPI.HelpDesk.Application.Interfaces.Security;
using OPI.HelpDesk.Application.Interfaces.UnitOfWorks;
using OPI.HelpDesk.Application.Interfaces.Users;
using OPI.HelpDesk.Application.Specifications;
using OPI.HelpDesk.Application.Specifications.Users;
using OPI.HelpDessk.Domain.Entities;

namespace OPI.HelpDesk.Application.Services.Users
{
    public class UserService(IUnitOfWork uow, IAuthService authService) : IUserService
    {
        public async Task<UserResponseDto> AddUser(UserAddRequestDto request, CancellationToken ct)
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
                Role = request.Role
            };

            if (request.Specialities is not null) newUser.Specialities = request.Specialities;
            if (request.MaxTicket is not null) newUser.MaxTicket = request.MaxTicket.Value;

            await uow.Repository<User>().AddAsync(newUser, ct);
            await uow.SaveChangesAsync();
            uow.Dispose();

            return ToResponse(newUser);
        }

        public async Task<GetAllResponseDto<UserResponseDto>> GetAllUsers(GetAllUserQueryDto query, CancellationToken ct)
        {
            var spe = new UserSpecificationFilter(query);
            var users = await uow.Repository<User>().GetAllAsync(spe, ct);

            return new GetAllResponseDto<UserResponseDto> {
                Data = users.Select(ToResponse),
                Count = await uow.Repository<User>().CountAsync(),
                Page = query.Page,
                PageSize = query.PageSize,
            };
        }

        public async Task<UserResponseDto> GetByIdUser(Guid id, CancellationToken ct)
        {
            var spec = new SpecificationBuilder<User>()
                .Where(u => u.Id == id)
                .Include(u => u.AssignedTickets)
                .Include(u => u.CreateTickets)
                .Include(u => u.Comments);
            var user = await uow.Repository<User>().GetAsync(spec, ct);

            if (user == null)
                throw new Exception("Userio no existe en base de datos.");

            return ToResponse(user);
        }

        public async Task<UserResponseDto> UpdateUser(UserEditRequestDto request, CancellationToken ct)
        {
            var spec = new SpecificationBuilder<User>()
                .Where(u => u.Id == request.Id);
            var userUpdated = await uow.Repository<User>().GetAsync(spec, ct);

            if (userUpdated == null)
                throw new Exception("Ek usuario a actualizar no se encuentra registrado.");

            if (request.FirstName is not null) userUpdated.FirstName = request.FirstName;
            if(request.LastName is not null) userUpdated.LastName = request.LastName;
            if(request.Email is not null) userUpdated.Email = request.Email;
            if(request.Password is not null)
            {
                var hasPassword = authService.HashPassword(request.Password);
                userUpdated.HashPassword = hasPassword;
            }
            if(request.Role is not null) userUpdated.Role = request.Role.Value;
            if (request.Specialities is not null) userUpdated.Specialities = request.Specialities;
            if(request.MaxTicket is not null) userUpdated.MaxTicket = request.MaxTicket.Value;
            if(request.Enable is not null) userUpdated.Enable = request.Enable.Value;

            uow.Repository<User>().Update(userUpdated);
            await uow.SaveChangesAsync();
            uow.Dispose();

            return ToResponse(userUpdated);
        }

        private static UserResponseDto ToResponse(User user) => new()
        {
            Id = user.Id,
            Name = string.Concat(user.FirstName, " ", user.LastName),
            Email = user.Email,
            Role = user.Role.ToString(),
            Specialities = user.Specialities.Select(s => ((int)s).ToString()).ToList(),
            MaxTicket = user.MaxTicket,
            AssignedTickets = user.AssignedTickets,
            CreateTickets = user.CreateTickets,
            Comments = user.Comments
        };
    }
}
