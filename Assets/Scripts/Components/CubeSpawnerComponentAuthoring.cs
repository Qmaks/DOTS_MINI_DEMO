using Unity.Entities;
using UnityEngine;

namespace Components
{
    public struct CubeSpawnerComponent : IComponentData
    {
        public Entity cube1Prefab;
        public Entity cube2Prefab;
    }
    
    public class CubeSpawnerComponentAuthoring : MonoBehaviour
    {
        public GameObject Cube1Prefab;
        public GameObject Cube2Prefab;

        public class PlayerSpawnerComponentBaker : Baker<CubeSpawnerComponentAuthoring>
        {
            public override void Bake(CubeSpawnerComponentAuthoring authoring)
            {
                AddComponent(new CubeSpawnerComponent
                {
                    cube1Prefab = GetEntity(authoring.Cube1Prefab),
                    cube2Prefab = GetEntity(authoring.Cube2Prefab)
                });
            }
        }
    }
}