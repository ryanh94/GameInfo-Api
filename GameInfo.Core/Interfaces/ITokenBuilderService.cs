using GameInfo.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameInfo.Core.Interfaces
{
    public interface ITokenBuilderService
    {
        AuthToken GetToken(int userId);
    }
}
