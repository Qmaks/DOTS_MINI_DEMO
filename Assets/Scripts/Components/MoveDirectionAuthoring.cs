using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Components
{
    public struct MoveDirection : IComponentData
    {
        public float3 value;
    }

    public class MoveDirectionAuthoring : MonoBehaviour
    {
        public float3 Value;

        public class MoveDirectionBaker : Baker<MoveDirectionAuthoring>
        {
            public override void Bake(MoveDirectionAuthoring authoring)
            {
                var entity = GetEntity(authoring,TransformUsageFlags.Dynamic);
                
                AddComponent(entity,new MoveDirection { value = authoring.Value });
            }
        }
    }
}