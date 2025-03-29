using AtomServer.database;
using AtomServer.Models;
using Microsoft.EntityFrameworkCore;

namespace AtomServer.Helpers
{
    public static class AuthHelper
    {
        public static async Task<object> CheckAuth(string token, DataContext context)
        {
            if (String.IsNullOrEmpty(token))
            {
                return new ServerResponseModel
                {
                    code = 401,
                    result = "Пользователь неавторизован"
                };
            }

            var user = await context.Users.FirstOrDefaultAsync(x=>x.Token.Value.ToString() == token);

            if (user == null) {

                return new ServerResponseModel
                {
                    code = 401,
                    result = "Неверный токен"
                };
            }

            return user;
        }
    }
}
