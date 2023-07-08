using Components;
using Unity.Entities;

namespace Systems
{
    public partial class TimerSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var deltaTime = SystemAPI.Time.DeltaTime;
            ;
            foreach (var timerComponent in SystemAPI.Query<RefRW<TimerComponent>>())
            {
                if (timerComponent.ValueRW.remainingTime > 0)
                    timerComponent.ValueRW.remainingTime -= deltaTime;
            }
        }
    }
}