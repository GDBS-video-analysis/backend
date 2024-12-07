namespace Web.DTOs
{
    public class PaginatedVM<T>
    {
        public int Page { get; set; }
        public int Count { get; set; }
        public List<T> Nodes { get; set; } = [];
    }
}
