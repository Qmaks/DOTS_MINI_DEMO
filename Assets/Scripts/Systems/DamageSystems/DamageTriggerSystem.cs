using Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;

namespace Systems
{
    [RequireMatchingQueriesForUpdate]
    [UpdateInGroup(typeof(PhysicsSystemGroup))]
    [UpdateAfter(typeof(PhysicsSimulationGroup))]
    public partial struct DamageTriggerSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<SimulationSingleton>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var simulation = SystemAPI.GetSingleton<SimulationSingleton>();
            simulation.AsSimulation().FinalSimulationJobHandle.Complete();
            
            var triggerJob = new TriggerJob
            {
                Death = SystemAPI.GetComponentLookup<Death>(),
                DealDamage = SystemAPI.GetComponentLookup<DealDamage>(),
                Damage = SystemAPI.GetBufferLookup<Damage>(),
            };
            
            state.Dependency = triggerJob.Schedule(simulation, state.Dependency);
            state.Dependency.Complete();
        }
        
        [BurstCompile]
        private struct TriggerJob : ITriggerEventsJob
        {
            public ComponentLookup<Death> Death;
            public ComponentLookup<DealDamage> DealDamage;
            public BufferLookup<Damage> Damage;
            
            [BurstCompile]
            public void Execute(TriggerEvent triggerEvent)
            {
                if (DealDamage.HasComponent(triggerEvent.EntityA))
                {
                    if (!Death.HasComponent(triggerEvent.EntityB) && Damage.HasBuffer(triggerEvent.EntityB))
                    {
                        Damage[triggerEvent.EntityB].Add(new Damage
                        {
                            value = DealDamage[triggerEvent.EntityA].value
                        });
                    }
                }
                
                if (DealDamage.HasComponent(triggerEvent.EntityB))
                {
                    if (!Death.HasComponent(triggerEvent.EntityA) && Damage.HasBuffer(triggerEvent.EntityA))
                    {
                        Damage[triggerEvent.EntityA].Add(new Damage
                        {
                            value = DealDamage[triggerEvent.EntityB].value
                        });
                    }
                }
            }
        }
    }
}