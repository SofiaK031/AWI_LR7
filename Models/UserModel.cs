namespace WebApplicationLR7.Models
{
    public class UserModel
    {
        public Guid Id { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public UserModel()
        {
            Id = Guid.NewGuid();
        }
    }
}