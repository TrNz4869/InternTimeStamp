namespace InternTimeStamp.Models
{
    public class TimeStamp
    {
        public string Name { get; set; }
        public DateTime CheckinTime { get; set; }
        public DateTime CheckoutTime { get; set; }
        public string Remark { get; set; }
        public DateTime LastModifyDate { get; set; }
    }
}
