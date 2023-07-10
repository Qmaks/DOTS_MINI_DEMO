using Components;
using Unity.Burst;
using Unity.Entities;

namespace Systems
{
    [UpdateAfter(typeof(ResolveDamageSystem))]
    public partial class DestroyOnDeathSystem : SystemBase
    {
        private EndSimulationEntityCommandBufferSystem ecbSystem;

        protected override void OnCreate()
        {
            base.OnCreate();
            ecbSystem = World
                .GetOrCreateSystemManaged<EndSimulationEntityCommandBufferSystem>();
        }
        
        protected override void OnUpdate()
        {
            var ecb = this.ecbSystem.CreateCommandBuffer();

            Entities.WithAll<Death>().ForEach((Entity entity) =>
            {
                ecb.DestroyEntity(entity);
            }).Run();
        }
    }
}