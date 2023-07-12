using Unity.Entities;
using UnityEngine;

namespace Components
{
    public struct Damage : IBufferElementData
    {
        public float value;
    }
    
    public struct DealDamage : IComponentData
    {
        public float value;
    }

    public class DealDamageAuthoring : MonoBehaviour
    {
        public float Value;

        public class DealDamageBaker : Baker<DealDamageAuthoring>
        {
            public override void Bake(DealDamageAuthoring authoring)
            {
                var entity = GetEntity(authoring,TransformUsageFlags.Dynamic);
                
                AddComponent(entity,new DealDamage { value = authoring.Value });
            }
        }
    }
}