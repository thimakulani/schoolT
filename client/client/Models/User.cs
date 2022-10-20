namespace client.Models
{
    public class User
    {
        public string Id { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string AccountStatus { get; set; }
        public string UserType { get; set; }
        public object ImageUrl { get; set; }
        public string OnlineStatus { get; set; }
    }
}
