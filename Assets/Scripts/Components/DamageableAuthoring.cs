using Unity.Entities;
using UnityEngine;

namespace Components
{
    public struct Health : IComponentData
    {
        public float value;
    }

    public class DamageableAuthoring : MonoBehaviour
    {
        public float StartingHealth;

        public class DamageableBaker : Baker<DamageableAuthoring>
        {
            public override void Bake(DamageableAuthoring authoring)
            {
                var entity = GetEntity(authoring, TransformUsageFlags.Dynamic);
                
                AddComponent(entity,new Health
                {
                    value = authoring.StartingHealth
                });
                
                AddBuffer<Damage>(entity);
            }
        }
    }
}