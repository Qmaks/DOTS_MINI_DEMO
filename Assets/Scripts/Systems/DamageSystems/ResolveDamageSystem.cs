using Components;
using Unity.Burst;
using Unity.Entities;

namespace Systems
{
    public partial struct ResolveDamageSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state) 
        {
            var ecbSystem = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);
            
            new ResolveDamageJob
            {
                Death = SystemAPI.GetComponentLookup<Death>(),
                CommandBuffer = ecbSystem
            }.Schedule();
        }

        [BurstCompile]
        private partial struct ResolveDamageJob : IJobEntity
        {
            public ComponentLookup<Death> Death;
            public EntityCommandBuffer CommandBuffer;

            public void Execute(Entity entity,ref DynamicBuffer<Damage> damages,ref Health health)
            {
                if (Death.HasComponent(entity))
                    return;
                
                foreach (var damage in damages)
                {
                    health.value -= damage.value;

                    if (health.value <= 0)
                    {
                        health.value = 0;
                        CommandBuffer.AddComponent<Death>(entity);
                        break;
                    }
                }
                
                damages.Clear();
            }
        }        
    }
}