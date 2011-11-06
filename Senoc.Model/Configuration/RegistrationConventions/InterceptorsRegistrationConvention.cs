using System;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;
using StructureMap.Interceptors;
using StructureMap.TypeRules;

namespace Senoc.Model.Configuration.RegistrationConventions
{
    public class InterceptorsRegistrationConvention : IRegistrationConvention
    {
        public void Process(Type type, Registry registry)
        {
            if (type.IsConcreteAndAssignableTo(typeof(TypeInterceptor)))
                registry.RegisterInterceptor((TypeInterceptor)Activator.CreateInstance(type));
        }
    }
}