using Senoc.Model.Configuration.RegistrationConventions;
using Senoc.Model.Eventing;
using Senoc.Model.Primitives;
using StructureMap.Configuration.DSL;

namespace Senoc.Model.Configuration
{
    public class DomainRegistry : Registry
    {
        public DomainRegistry()
        {
            Scan(x =>
            {
                x.TheCallingAssembly();
                x.Convention<EvaluationMetricsRegistrationConvention>();
                x.Convention<InterceptorsRegistrationConvention>();
                x.WithDefaultConventions();
            });

            For<IDistributor>().Singleton().Use<Distributor>();
            For<IClock>().Singleton().Use<Clock>();
        }
    }
}