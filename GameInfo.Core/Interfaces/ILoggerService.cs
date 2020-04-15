using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GameInfo.Core.Interfaces
{
    public interface ILoggerService
    {
        Task LogRequest(int userid, string requestbody, string responsebody);
    }
}
