using GameInfo.Core.Model;
using GameInfo.Infrastructure.Repository.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GameInfo.Tests.ServiceTests
{
    public abstract class BaseServiceTests
    {
        public Mock<IDBFactory> _DBFactory { get; private set; }
        public Mock<IRepository> _Repo { get; private set; }

        [TestInitialize]
        public void Init()
        {
            _DBFactory = new Mock<IDBFactory>();
            _Repo = new Mock<IRepository>();
            _DBFactory.Setup(x => x.GetInstance()).Returns(_Repo.Object);
        }
    }
}
