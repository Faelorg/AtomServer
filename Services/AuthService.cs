using AtomServer.database;
using AtomServer.Helpers;
using AtomServer.Models;
using Microsoft.EntityFrameworkCore;

namespace AtomServer.Services
{
    public interface IAuthService
    {
        Task<ServerResponseModel> Register(UserRequestModel model);
        Task<ServerResponseModel> Login(AuthRequestModel model);
        Task<ServerResponseModel> Logout();

    }

    public class AuthService : IAuthService
    {
        private readonly IHttpContextAccessor httpContext;
        private readonly DataContext context;
        public AuthService(DataContext context, IHttpContextAccessor httpContext)
        {
            this.context = context;
            this.httpContext = httpContext;
        }

        public async Task<ServerResponseModel> Login(AuthRequestModel model)
        {
            try
            {
                var user = await context.Users.FirstOrDefaultAsync(x => (x.Login == model.Login || x.Email == model.Login) && x.Password == model.Password);

                if (user == null) {
                    return new ServerResponseModel
                    {
                        code = 401,
                        result = new { message = "Неверный логин или пароль" }
                    };
                }

                user.Token = Guid.NewGuid();

                context.Update(user);
                await context.SaveChangesAsync();

                return new ServerResponseModel
                {
                    code = 200,
                    result = new AuthResponseModel
                    {
                        token = user.Token.Value
                    }
                };
            }
            catch (Exception ex)
            {
                return new ServerResponseModel
                {
                    code = 500,
                    result = new { message = ex.Message },
                };
            }
        }

        public async Task<ServerResponseModel> Logout()
        {
            try
            {
                var token = httpContext.HttpContext.Request.Headers["Authorization"];

                var auth = await AuthHelper.CheckAuth(token, context);

                if (auth is ServerResponseModel)
                {
                    return auth as ServerResponseModel;
                }

                var user = auth as User;

                user.Token = null;

                context.Update(user);

                await context.SaveChangesAsync();

                return new ServerResponseModel
                {
                    code = 200,
                    result = new { message = "Успех" }
                };
            }
            catch (Exception ex)
            {
                return new ServerResponseModel
                {
                    code = 500,
                    result = new { message = ex.Message },
                };
            }
        }

        public async Task<ServerResponseModel> Register(UserRequestModel model)
        {
            try
            {
                var user = await context.Users.FirstOrDefaultAsync(x => x.Login == model.Login || x.Email == model.Email);

                if (user != null)
                {
                    return new ServerResponseModel
                    {
                        code = 400,
                        result = new { message = "Пользователь с таким логином или email уже зарегистрирован" }
                    };
                }
                var role = await context.Roles.FirstOrDefaultAsync(x => x.Id == model.IdRole);
                if (role == null)
                {
                    role = await context.Roles.FirstOrDefaultAsync(x => x.Code == "user");
                }
                user = new User
                {
                    Id = Guid.NewGuid(),
                    Login = model.Login,
                    Email = model.Email,
                    Password = model.Password,
                    Firstname = model.Firstname,
                    Middlename = model.Middlename,
                    Lastname = model.Lastname,
                    IdRole = role.Id,
                    Token = Guid.NewGuid(),
                };

                await context.Users.AddAsync(user);

                await context.SaveChangesAsync();

                return new ServerResponseModel
                {
                    code = 200,
                    result = new AuthResponseModel
                    {
                        token = user.Token.Value
                    }
                };
            }
            catch (Exception ex)
            {
                return new ServerResponseModel
                {
                    code = 500,
                    result = new { message = ex.Message },
                };
            }
        }
    }
}
