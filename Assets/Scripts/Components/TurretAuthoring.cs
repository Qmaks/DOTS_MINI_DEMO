using Unity.Entities;
using UnityEngine;

namespace Components
{
    public struct Turret : IComponentData
    {
        public Entity CannonBallPrefab;
        public Entity CannonBallSpawn;
    }

    public class TurretAuthoring : MonoBehaviour
    {
        public GameObject CannonBallPrefab;
        public Transform CannonBallSpawn;
        public float shootDuration;
        
        public class TurretBaker : Baker<TurretAuthoring>
        {
            public override void Bake(TurretAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                
                AddComponent(entity,new Turret
                {
                    CannonBallPrefab = GetEntity(authoring.CannonBallPrefab,TransformUsageFlags.Dynamic),
                    CannonBallSpawn = GetEntity(authoring.CannonBallSpawn,TransformUsageFlags.Dynamic)
                });
                
                AddComponent(entity,new Timer(authoring.shootDuration));
            }
        }
    }
}