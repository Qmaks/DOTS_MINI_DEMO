using Unity.Entities;
using Unity.Transforms;

namespace Components.Aspects
{
    public readonly partial struct MoveInDirectionAspect : IAspect
    {
        private readonly RefRW<LocalTransform> localTransform;
        private readonly RefRO<Speed> speed; 
        private readonly RefRO<MoveDirection> direction;

        public void Move(float dT)
        {
            localTransform.ValueRW.Position += direction.ValueRO.value * dT * speed.ValueRO.value;
        }
    }
}