namespace client.Models
{
    public class User
    {
        public string Id { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string AccountStatus { get; set; }
        public string UserType { get; set; }
        public object ImageUrl { get; set; }
        public string OnlineStatus { get; set; }
    }
}
