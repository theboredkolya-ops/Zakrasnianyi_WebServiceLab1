namespace WebServiceLab1.Models
{
    public class RegistrationModel
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string? Phone { get; set; }

        public int Age { get; set; }

        public string Password { get; set; } = string.Empty;

        public string ConfirmPassword { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public string? Address { get; set; }

        public string? Website { get; set; }
    }
}