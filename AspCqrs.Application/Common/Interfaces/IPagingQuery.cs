namespace AspCqrs.Application.Common.Interfaces
{
    public interface IPagingQuery
    {
        public int Page { get; set; }
        
        public int PageSize { get; set; }
    }
}