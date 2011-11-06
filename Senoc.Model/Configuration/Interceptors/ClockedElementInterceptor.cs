using System;
using Senoc.Model.Primitives;
using StructureMap;
using StructureMap.Interceptors;
using StructureMap.TypeRules;

namespace Senoc.Model.Configuration.Interceptors
{
    public class ClockedElementInterceptor : TypeInterceptor
    {
        public object Process(object target, IContext context)
        {
            var clockedElement = (IClockedElement)target;
            context.GetInstance<IClock>().Subscribe(clockedElement);

            return target;
        }

        public bool MatchesType(Type type)
        {
            return type.IsConcreteAndAssignableTo(typeof(IClockedElement));
        } 
    }
}