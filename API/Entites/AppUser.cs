namespace API.Entites
{
    public class AppUser
    {
        public int Id { get; set; }
        public string UserName { get; set; }

        public byte[] Passwordhash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}