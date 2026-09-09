namespace SecurePasswordGenerator.Models
{
    public class Password
    {
        public int Id { get; set; }

        public string Value { get; set; } = string.Empty;

        public int Length { get; set; }

        public string Strength { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; }
    }
}