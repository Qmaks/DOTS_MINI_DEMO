using Unity.Entities;
using UnityEngine;

namespace Components
{
    public struct Speed : IComponentData
    {
        public float value;
    }

    public class SpeedAuthoring : MonoBehaviour
    {
        public float Value;
        public class SpeedBaker : Baker<SpeedAuthoring>
        {
            public override void Bake(SpeedAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity,new Speed { value = authoring.Value });
            }
        }
    }
}