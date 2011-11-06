using System;
using Senoc.Model.Evaluators;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;
using StructureMap.TypeRules;

namespace Senoc.Model.Configuration.RegistrationConventions
{
    public class EvaluationMetricsRegistrationConvention : IRegistrationConvention
    {
        public void Process(Type type, Registry registry)
        {
            if (type.IsConcreteAndAssignableTo(typeof(IEvaluateMetrics)))
                registry.AddType(typeof(IEvaluateMetrics), type);
        }
    }
}