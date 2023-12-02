namespace BuildingBlocks.Domain
{
    public class PaginationResult<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public List<T> Data { get; set; }

        public PaginationResult(int pageNumber, int pageSize, int totalRecords, List<T> data)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalRecords = totalRecords;
            Data = data;
        }
    }
}
