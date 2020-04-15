using GameInfo.Core.Interfaces;
using GameInfo.Core.Models.Entities;
using GameInfo.Infrastructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GameInfo.Infrastructure.Services
{
    public class LoggerService : ILoggerService
    {
        private readonly IDBFactory _repo;

        public LoggerService(IDBFactory repo)
        {
            _repo = repo;
        }
        public async Task LogRequest(int userid, string requestbody, string responsebody)
        {
            try
            {
                using (var repo = _repo.GetInstance())
                {
                    repo.Add(new Audit
                    {
                        Created = DateTimeOffset.UtcNow,
                        Request = requestbody,
                        Response = responsebody,
                        UserId = userid
                    });

                    await repo.Commit();
                }
            }
            catch (Exception)
            {
                // Supress errors to enable request to pass
            }
        }
    }
}
