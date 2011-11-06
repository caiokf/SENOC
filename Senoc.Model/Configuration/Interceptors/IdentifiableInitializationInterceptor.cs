using System;
using Senoc.Model.Primitives;
using StructureMap;
using StructureMap.Interceptors;
using StructureMap.TypeRules;

namespace Senoc.Model.Configuration.Interceptors
{
    public class IdentifiableInitializationInterceptor : TypeInterceptor
    {
        public object Process(object target, IContext context)
        {
            ((IIdentifiable)target).Id = Guid.NewGuid();
            return target;
        }

        public bool MatchesType(Type type)
        {
            return type.IsConcreteAndAssignableTo(typeof(IIdentifiable));
        } 
    }
}