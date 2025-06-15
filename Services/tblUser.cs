using System.ComponentModel.DataAnnotations;

namespace WpfEfAuthen.Services
{
    public  class tblUser
    {
        [Key]
        public int Id { get; set; }

        public string? Username { get; set; }

        public string? PasswordHash { get; set; }

        public string? UserRole { get; set; }
    }
}
