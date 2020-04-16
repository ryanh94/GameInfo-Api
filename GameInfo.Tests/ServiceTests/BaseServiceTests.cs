using GameInfo.Core.Model;
using GameInfo.Infrastructure.Repository.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GameInfo.Tests.ServiceTests
{
    public abstract class BaseServiceTests
    {
        public Mock<IRepository> _Repo { get; private set; }

        [TestInitialize]
        public void Init()
        {
            _Repo = new Mock<IRepository>();
        }
    }
}
