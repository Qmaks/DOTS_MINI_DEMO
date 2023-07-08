using Components;
using Components.Aspects;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Systems
{
    public partial class ShootingSystem : SystemBase  
    {
        protected override void OnUpdate()
        {
            foreach (var aspect in SystemAPI.Query<TurretAspect>())
            {
                if (aspect.shootTimerComponent.ValueRO.IsDone)
                {
                    var entity = EntityManager.Instantiate(aspect.CannonBallPrefab);
                
                    EntityManager.SetComponentData(entity, new LocalTransform
                    {
                        Position = SystemAPI.GetComponent<LocalToWorld>(aspect.CannonBallSpawn).Position,
                        Rotation = quaternion.identity,
                        Scale    = SystemAPI.GetComponent<LocalTransform>(aspect.CannonBallPrefab).Scale
                    });
                    
                    EntityManager.SetComponentData(entity, new MoveDirection()
                    {
                        value = SystemAPI.GetComponent<MoveDirection>(aspect.entity).value
                    });
                    
                    aspect.shootTimerComponent.ValueRW.Reset();
                }
            }
        }
    }
}