namespace OPI.HelpDesk.Application.Dtos.Shares
{
    public class GetAllResponseDto<T>
    {
        public IEnumerable<T> Data { get; set; } = default;
        public int Count { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}

