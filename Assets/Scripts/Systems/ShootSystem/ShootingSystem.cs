using Components;
using Components.Aspects;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Systems
{
    [BurstCompile]
    public partial struct ShootingSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
        }

        private EntityCommandBuffer ecb;
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);

            new ShootJob()
            {
                Ecb = ecb,
                LocalToWorldComponent = SystemAPI.GetComponentLookup<LocalToWorld>(),
                LocalTransformComponent = SystemAPI.GetComponentLookup<LocalTransform>(),
                MoveDirectionComponent = SystemAPI.GetComponentLookup<MoveDirection>()
            }.Schedule();
        }
        
        [BurstCompile]
        private partial struct ShootJob : IJobEntity
        {
            public EntityCommandBuffer Ecb;
            public ComponentLookup<LocalToWorld> LocalToWorldComponent;
            public ComponentLookup<LocalTransform> LocalTransformComponent;
            public ComponentLookup<MoveDirection> MoveDirectionComponent;
            
            [BurstCompile]
            void Execute(TurretAspect aspect)
            {
                if (aspect.shootTimerComponent.ValueRO.IsDone)
                {
                    var entity = Ecb.Instantiate(aspect.CannonBallPrefab);
                    
                    Ecb.SetComponent(entity, new LocalTransform
                    {
                        Position = LocalToWorldComponent.GetRefRO(aspect.CannonBallSpawn).ValueRO.Position,
                        Rotation = quaternion.identity,
                        Scale    = LocalTransformComponent.GetRefRO(aspect.CannonBallPrefab).ValueRO.Scale
                    });
                    
                    Ecb.SetComponent(entity, new MoveDirection()
                    {
                        value = MoveDirectionComponent.GetRefRO(aspect.entity).ValueRO.value
                    });
                    
                    aspect.shootTimerComponent.ValueRW.Reset();
                }
            }
        }
    }
}