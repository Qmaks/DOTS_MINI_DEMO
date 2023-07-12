using Unity.Entities;
using UnityEngine;

namespace Components
{
    public struct CubeSpawner : IComponentData
    {
        public Entity cube1Prefab;
        public Entity cube2Prefab;
    }
    
    public class CubeSpawnerAuthoring : MonoBehaviour
    {
        public GameObject Cube1Prefab;
        public GameObject Cube2Prefab;

        public class PlayerSpawnerBaker : Baker<CubeSpawnerAuthoring>
        {
            public override void Bake(CubeSpawnerAuthoring authoring)
            {
                var entity = GetEntity(authoring, TransformUsageFlags.None);
                
                AddComponent(entity,new CubeSpawner
                {
                    cube1Prefab = GetEntity(authoring.Cube1Prefab,TransformUsageFlags.Dynamic),
                    cube2Prefab = GetEntity(authoring.Cube2Prefab,TransformUsageFlags.Dynamic)
                });
            }
        }
    }
}