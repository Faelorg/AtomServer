namespace AtomServer.Models
{
    public class AuthRequestModel
    {
        public string Login { get; set; } = null!;

        public string Password { get; set; } = null!;
    }

    public class AuthResponseModel
    {
        public Guid token { get; set; }
    }
}
