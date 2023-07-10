using Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;

namespace Systems
{
    
    [RequireMatchingQueriesForUpdate]
    [UpdateInGroup(typeof(PhysicsSystemGroup))]
    [UpdateAfter(typeof(DamageTriggerSystem))]
    public partial struct DestroyOnContactSystem : ISystem
    {
         [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<SimulationSingleton>();
            state.RequireForUpdate<BeginFixedStepSimulationEntityCommandBufferSystem.Singleton>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecbBegin = SystemAPI.GetSingleton<BeginFixedStepSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);
            
            var simulation = SystemAPI.GetSingleton<SimulationSingleton>();
            simulation.AsSimulation().FinalSimulationJobHandle.Complete();
            
            var triggerJob = new TriggerJob
            {
                DestroyOnContact = SystemAPI.GetComponentLookup<DestroyOnContact>(),
                EcbBegin = ecbBegin
            };
            
            state.Dependency = triggerJob.Schedule(simulation, state.Dependency);
            state.Dependency.Complete();
        }
        
        [BurstCompile]
        private struct TriggerJob : ITriggerEventsJob
        {
            public ComponentLookup<DestroyOnContact> DestroyOnContact;
            
            public EntityCommandBuffer EcbBegin;
            
            public void Execute(TriggerEvent triggerEvent)
            {
                if (DestroyOnContact.HasComponent(triggerEvent.EntityA))
                    EcbBegin.DestroyEntity(triggerEvent.EntityA);

                if (DestroyOnContact.HasComponent(triggerEvent.EntityB))
                    EcbBegin.DestroyEntity(triggerEvent.EntityB);
            }
        }
    }
}