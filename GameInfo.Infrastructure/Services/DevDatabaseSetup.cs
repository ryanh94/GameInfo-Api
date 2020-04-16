using GameInfo.Core.Models.Entities;
using GameInfo.Infrastructure.Repository.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameInfo.Infrastructure.Services
{
    public class DevDatabaseSetup
    {
        private readonly GameInfoContext _context;
        private readonly IWebHostEnvironment _environment;

        public DevDatabaseSetup(GameInfoContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public void Setup()
        {
            if (!_environment.IsDevelopment())
            {
                throw new System.Exception("Cannot seed a dev database in a non-development environmetn");
            }
            _context.Database.Migrate();
            if (_context.User.All(u => u.Id != 1))
            {
                _context.Add(new User
                {
                    Username = "dev",
                    Password = "AJwVUxcnS1/My268csKdzqIthWksh2t6idwybqbwWM6pM01SCSANatdOi5pGksrbGw==" /*Password1*/
                }); 
            }
            _context.SaveChanges();
        }
    }
}
