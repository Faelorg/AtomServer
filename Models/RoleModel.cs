namespace AtomServer.Models
{
    public class RoleRequestModel
    {
        public string Name { get; set; } = null!;

        public string Code { get; set; } = null!;
    }

    public class RoleResponseModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = null!;

        public string Code { get; set; } = null!;
    }
}
