using AutoMapper;
using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            List<User> list = new List<User>();
            var user = new User()
            {
                Id = 1,
                Age = 10,
                Name = "Jeffcky"
            };
            list.Add(user);
            var config = new MapperConfiguration(cfg => cfg.CreateMap<List<User>, List<UserDTO>>());
            //或者Mapper.Initialize(cfg => cfg.CreateMap<User, UserDTO>());
            var mapper = config.CreateMapper();
            //或者var mapper = new Mapper(config);
            //最终调用Map方法进行映射
            var userDto = mapper.Map<User, UserDTO>(user);
            var userDTO = mapper.Map<List<User>, List<UserDTO>>(list);


            Console.ReadKey();
        }

        public class User
        {
            public int Id { get; set; }
            public int Age { get; set; }
            public string Name { get; set; }
        }

        public class UserDTO
        {
            public int Id { get; set; }
            public int Age { get; set; }
            public string Name { get; set; }
        }

    }
}
