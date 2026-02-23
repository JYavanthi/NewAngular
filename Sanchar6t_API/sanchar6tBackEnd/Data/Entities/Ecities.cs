namespace sanchar6tBackEnd.Data.Entities
{
    public class Ecities
    {
        public char Flag { get; set; } = 'I'; // 'I' for insert
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDt { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDt { get; set; }

    }
}
