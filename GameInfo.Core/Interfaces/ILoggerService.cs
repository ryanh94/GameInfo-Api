using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GameInfo.Core.Interfaces
{
    public interface ILoggerService
    {
        void LogRequest(int userId, string requestBody, string responseBody);
    }
}
