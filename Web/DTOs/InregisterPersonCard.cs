namespace Web.DTOs
{
    public class InregisterPersonCard
    {
        public long UnregisterPersonID { get; set; }
        public List<UnregisterPersonVideoFileMarks> VideoFileMarks { get; set; } = [];
    }

    public class UnregisterPersonVideoFileMarks
    {
        public DateTime Mark { get; set; }
        public long PhotoID { get; set; }
    }
}
