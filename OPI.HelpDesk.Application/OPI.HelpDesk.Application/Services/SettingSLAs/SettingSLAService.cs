using OPI.HelpDesk.Application.Dtos.SettingSLAs;
using OPI.HelpDesk.Application.Dtos.Shares;
using OPI.HelpDesk.Application.Interfaces.SettingSLAs;
using OPI.HelpDesk.Application.Interfaces.UnitOfWorks;
using OPI.HelpDesk.Application.Specifications;
using OPI.HelpDesk.Application.Specifications.SettingSLAs;
using OPI.HelpDessk.Domain.Entities;
using OPI.HelpDessk.Domain.Entities.Enums;

namespace OPI.HelpDesk.Application.Services.SettingSLAs
{
    public class SettingSLAService(IUnitOfWork uow) : ISettingSLA
    {
        public async Task<SettingSLAResponseDto> AddSettingSla(SettingSLAAddRequestDto request, CancellationToken ct)
        {
            Guid id = Guid.NewGuid();
            SettingSLA newSetting = new SettingSLA
            {
                Id = id,
                Priority = (TicketPriorityEnum)Enum.Parse(typeof(TicketPriorityEnum), request.Priority, true),
                Categority = (TicketCategoriesEnum)Enum.Parse(typeof(TicketCategoriesEnum), request.Category, true),
                LimitTime = request.LimitTime
            };
            await uow.Repository<SettingSLA>().AddAsync(newSetting, ct);
            await uow.SaveChangesAsync();
            uow.Dispose();

            return ToResponse(newSetting);

        }

        public async Task<GetAllResponseDto<SettingSLAResponseDto>> GetAllSettingSLAs(GetSettingSLAsQueryDto query, CancellationToken ct)
        {
            var spec = new SettingSLASpecificatoFilter(query);
            var SettingSLAs = await uow.Repository<SettingSLA>().GetAllAsync(spec, ct);
            return new GetAllResponseDto<SettingSLAResponseDto>
            {
                Data = SettingSLAs.Select(ToResponse),
                Count = await uow.Repository<SettingSLA>().CountAsync(),
                Page = query.Page,
                PageSize = query.PageSize
            };
        }

        public async Task<SettingSLAResponseDto> GetByIdSettingSLA(Guid id, CancellationToken ct)
        {
            var spec = new SpecificationBuilder<SettingSLA>()
                .Where(ss => ss.Id == id);

            var settingSLA = await uow.Repository<SettingSLA>().GetAsync(spec, ct);

            if (settingSLA == null)
                throw new Exception("Configuración no insertada en la base de datos.");

            return ToResponse(settingSLA);
        }

        public async Task<SettingSLAResponseDto> UpdateSettingSLA(SettingSLAEditRequestDto request, CancellationToken ct)
        {
            var spec = new SpecificationBuilder<SettingSLA>()
                .Where(ss => ss.Id == request.Id);

            var settingSLAUpdated = await uow.Repository<SettingSLA>().GetAsync(spec, ct);

            if (settingSLAUpdated == null)
                throw new Exception("Configuración no insertada en la base de datos.");

            if (!string.IsNullOrEmpty(request.Priority))
            {
                var priority = (TicketPriorityEnum)Enum.Parse(typeof(TicketPriorityEnum), request.Priority, true);
                settingSLAUpdated.Priority = priority;
            }
            if (!string.IsNullOrEmpty(request.Category))
            {
                var category = (TicketCategoriesEnum)Enum.Parse(typeof(TicketCategoriesEnum), request.Category, true);
                settingSLAUpdated.Categority = category;
            }
            if(request.LimitTime.HasValue) settingSLAUpdated.LimitTime = request.LimitTime.Value;
            if(request.Enable.HasValue) settingSLAUpdated.Enable = request.Enable.Value;

            uow.Repository<SettingSLA>().Update(settingSLAUpdated);
            await uow.SaveChangesAsync();
            uow.Dispose();

            return ToResponse(settingSLAUpdated);
        }
        private static SettingSLAResponseDto ToResponse(SettingSLA settingSLA) => new()
        {
            Id = settingSLA.Id,
            Priority = settingSLA.Priority.ToString(),
            Category = settingSLA.Categority.ToString(),
            LimitTime = settingSLA.LimitTime,
            Enable = settingSLA.Enable,
        };
    }
}
