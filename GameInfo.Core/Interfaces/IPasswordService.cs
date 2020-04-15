using System;
using System.Collections.Generic;
using System.Text;

namespace GameInfo.Core.Interfaces
{
    public interface IPasswordService
    {
        string GeneratePassword(string password);
        bool ValidatePassword(string userSuppliedPassword, string hashedPassword);
    }
}
