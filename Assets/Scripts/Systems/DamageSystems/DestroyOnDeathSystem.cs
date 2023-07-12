using Components;
using Unity.Burst;
using Unity.Entities;

namespace Systems
{
    [BurstCompile]
    [UpdateAfter(typeof(ResolveDamageSystem))]
    public partial struct DestroyOnDeathSystem : ISystem
    {
        private EntityCommandBuffer ecb;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);
            
            new DestroyJob()
            {
                Ecb = ecb
            }.Schedule();
        }
        
        [BurstCompile]
        [WithAll(typeof(Death))]
        private partial struct DestroyJob:IJobEntity
        {
            public EntityCommandBuffer Ecb;
            
            void Execute(Entity entity)
            {
                Ecb.DestroyEntity(entity);
            }
        }
    }
}