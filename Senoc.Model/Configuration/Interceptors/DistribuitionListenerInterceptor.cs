using System;
using Senoc.Model.Eventing;
using StructureMap;
using StructureMap.Interceptors;
using StructureMap.TypeRules;

namespace Senoc.Model.Configuration.Interceptors
{
    public class DistribuitionListenerInterceptor: TypeInterceptor
    {
        public object Process(object target, IContext context)
        {
            context.GetInstance<IDistributor>().AddListener(target);
            return target;
        }

        public bool MatchesType(Type type)
        {
            return type.ImplementsInterfaceTemplate(typeof(IListenTo<>.All)) ||
                   type.ImplementsInterfaceTemplate(typeof(IListenTo<>.SentToMe)) ||
                   type.ImplementsInterfaceTemplate(typeof(IListenTo<>.ThatSatisfy));
        }        
    }
}