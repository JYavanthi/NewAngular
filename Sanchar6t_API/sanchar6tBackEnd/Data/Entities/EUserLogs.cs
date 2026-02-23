namespace sanchar6tBackEnd.Data.Entities
{
    public class EUserLogs
    {
        public char Flag { get; set; }

        public int UserlogID { get; set; }

        public int? UserID { get; set; }
         
        public DateTime? LoginTime { get; set; }

        public DateTime? LogoutTime { get; set; }
        public string? Token { get; set; }
        public int CreatedBy { get; set; }

    }
}
