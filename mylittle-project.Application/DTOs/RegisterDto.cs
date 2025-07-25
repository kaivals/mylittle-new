namespace mylittle_project.Application.DTOs
{
    // Path: Application/DTOs/RegisterDto.cs
    public class RegisterDto
    {
        public string Username { get; set; } // now required for login
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
    }


}

