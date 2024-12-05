namespace Web.DTOs
{
    public class UnknownVisitorCard
    {
        public long UnknownVisitorID { get; set; }
        public List<UnknownVisitorVideoFileMarks> VideoFileMarks { get; set; } = [];
    }

    public class UnknownVisitorVideoFileMarks
    {
        public TimeOnly Mark { get; set; }
        public long PhotoID { get; set; }
    }
}
