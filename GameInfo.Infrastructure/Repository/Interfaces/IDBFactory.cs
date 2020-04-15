using System;
using System.Collections.Generic;
using System.Text;

namespace GameInfo.Infrastructure.Repository.Interfaces
{
    public interface IDBFactory
    {
        IRepository GetInstance();
    }
}
