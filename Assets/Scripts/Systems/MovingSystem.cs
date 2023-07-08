using Components.Aspects;
using Unity.Burst;
using Unity.Entities;

namespace Systems
{
    [BurstCompile]
    public partial struct MovingSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state) { }
        [BurstCompile]
        public void OnDestroy(ref SystemState state) { }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            new MoveJob
            {
                dT = SystemAPI.Time.DeltaTime
            }.ScheduleParallel();
        }
    }

    [BurstCompile]
    public partial struct MoveJob : IJobEntity
    {
        public float dT;

        [BurstCompile]
        public void Execute(MoveInDirectionAspect moveInDirectionAspect)
        {
            moveInDirectionAspect.Move(dT);
        }
    }
}