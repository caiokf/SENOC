using NUnit.Framework;
using Senoc.Model.Configuration;
using Senoc.Model.Eventing;
using StructureMap;

namespace Senoc.Tests
{
    public class BaseTest
    {
        protected IDistributor distributor;
        
        [SetUp]
        public virtual void setup()
        {
            ObjectFactory.Configure(x => x.AddRegistry<DomainRegistry>());
            distributor = ObjectFactory.GetInstance<IDistributor>();
        }
    }
}