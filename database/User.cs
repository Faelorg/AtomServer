using System;
using System.Collections.Generic;

namespace AtomServer.database;

public partial class User
{
    public Guid Id { get; set; }

    public string Login { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Firstname { get; set; } = null!;

    public string Lastname { get; set; } = null!;

    public string? Middlename { get; set; }

    public Guid? Token { get; set; }

    public Guid IdRole { get; set; }

    public virtual Role IdRoleNavigation { get; set; } = null!;
}
