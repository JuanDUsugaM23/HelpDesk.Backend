namespace OPI.HelpDesk.Application.Interfaces.Auditoires
{
    public interface ICurrentUserService
    {
        Guid UserId { get; }
        string Role { get; }
    }
}
