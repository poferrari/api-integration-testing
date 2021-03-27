using System;
using System.Collections.Generic;

namespace ProjectWeather.Api.Models
{
    public class UserDto
    {
        public UserDto(string login, string password)
        {
            Id = Guid.NewGuid();
            Login = login;
            Password = password;
        }

        public Guid Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public static IEnumerable<UserDto> GetUsers()
        {
            return new List<UserDto>
            {
                new UserDto("admin", "admin123"),
                new UserDto("teste", "teste123")
            };
        }
    }
}
