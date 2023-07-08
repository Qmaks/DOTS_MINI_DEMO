using Unity.Burst;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;
using UnityEngine;

namespace Systems
{
    [RequireMatchingQueriesForUpdate]
    [UpdateInGroup(typeof(PhysicsSystemGroup))]
    [UpdateAfter(typeof(PhysicsSimulationGroup))]
    public partial struct DestroySystem : ISystem
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
            var ecbBegin = SystemAPI.GetSingleton<BeginFixedStepSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
            
            var simulation = SystemAPI.GetSingleton<SimulationSingleton>();
            simulation.AsSimulation().FinalSimulationJobHandle.Complete();
            
            var triggerJob = new TriggerJob
            {
                EcbBegin = ecbBegin
            };
            
            state.Dependency = triggerJob.Schedule(simulation, state.Dependency);
            state.Dependency.Complete();
        }
        
        [BurstCompile]
        private struct TriggerJob : ITriggerEventsJob
        {
            public EntityCommandBuffer EcbBegin;
            
            public void Execute(TriggerEvent triggerEvent)
            {
                EcbBegin.DestroyEntity(triggerEvent.EntityA);
                EcbBegin.DestroyEntity(triggerEvent.EntityB);
            }
        }
    }
}