using GameInfo.Core.Model;
using GameInfo.Infrastructure.Repository.Context;
using GameInfo.Infrastructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameInfo.Infrastructure.Repository
{
    public class DBFactory : IDBFactory
    {
        private readonly IOptions<AppSettings> _settings;

        public DBFactory(IOptions<AppSettings> settings)
        {
            _settings = settings;
        }
        public IRepository GetInstance()
        {
            var optionsBuilder = new DbContextOptionsBuilder<GameInfoContext>();

            optionsBuilder.UseSqlServer(_settings.Value.Connection);

            var context = new GameInfoContext(optionsBuilder.Options);

            return new GameInfoRepository(context);
        }
    }
}
