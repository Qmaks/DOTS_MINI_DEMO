using Unity.Entities;
using UnityEngine;

namespace Components
{
    public struct DestroyOnContact : IComponentData
    {
        
    }

    public class DestroyOnContactAuthoring : MonoBehaviour
    {
        public class DestroyOnContactBaker : Baker<DestroyOnContactAuthoring>
        {
            public override void Bake(DestroyOnContactAuthoring authoring)
            {
                AddComponent(new DestroyOnContact());
            }
        }
    }
}