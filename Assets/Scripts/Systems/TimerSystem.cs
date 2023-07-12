using Components;
using Unity.Burst;
using Unity.Entities;

namespace Systems
{
    [BurstCompile]
    public partial struct TimerSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            new TimerJob()
            {
                DeltaTime = SystemAPI.Time.DeltaTime
            }.ScheduleParallel();
        }

        [BurstCompile]
        private partial struct TimerJob : IJobEntity
        {
            public float DeltaTime; 
            void Execute(ref Timer timer)
            {
                if (timer.remainingTime > 0)
                    timer.remainingTime -= DeltaTime;
            }
        }
    }
}