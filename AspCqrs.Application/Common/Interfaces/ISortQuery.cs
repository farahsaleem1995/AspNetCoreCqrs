namespace AspCqrs.Application.Common.Interfaces
{
    public interface ISortQuery
    {
        public string SortBy { get; set; }
        
        public bool IsSortAscending { get; set; }
    }
}