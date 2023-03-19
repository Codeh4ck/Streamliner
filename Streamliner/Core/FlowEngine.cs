using System;
using System.Collections.Concurrent;
using Streamliner.Core.Base;
using Streamliner.Core.Utilities;
using Streamliner.Core.Utilities.Auditing;
using Streamliner.Definitions;

namespace Streamliner.Core
{
    public class FlowEngine : Runnable, IFlowEngine
    {
        private readonly FlowPlanFactoryBase _planFactory;
        private readonly ConcurrentDictionary<Guid, IFlowPlan> _flowPlans;
        public ILogger Logger { get; set; }
        public IFlowAuditLogger AuditLogger { get; set; }

        public FlowEngine(FlowPlanFactoryBase planFactory)
        {
            _planFactory = planFactory ?? throw new ArgumentNullException(nameof(planFactory));
            _flowPlans = new ConcurrentDictionary<Guid, IFlowPlan>();
        }

        private IFlowPlan CreateFlow(FlowDefinition definition)
        {
            definition.Logger ??= Logger;
            definition.AuditLogger ??= AuditLogger;

            IFlowPlan flowPlan = _planFactory.GeneratePlan(definition);

            if (!_flowPlans.TryAdd(definition.ServiceInfo.Id, flowPlan))
                throw new Exception("Plan already exists.");

            return flowPlan;
        }

        public IFlowPlan StartFlow(FlowDefinition definition)
        {
            IFlowPlan flowPlan = CreateFlow(definition);
            flowPlan.Start();

            return flowPlan;
        }

        public bool StopFlow(Guid planId)
        {
            if (!_flowPlans.TryRemove(planId, out IFlowPlan flowPlan))
                return false;

            flowPlan.Stop();
            return true;
        }

        protected override void OnStop()
        {
            foreach (IFlowPlan flowPlan in _flowPlans.Values)
                StopFlow(flowPlan.Definition.ServiceInfo.Id);
        }

        protected override void OnStart(object context = null) { }
    }
}