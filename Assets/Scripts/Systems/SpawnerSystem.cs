using Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace Systems
{
    public partial struct SpawerSystem : ISystem
    {
        private const int ARMY_AMMOUNT = 100;
        private const int LINE_LENGTH  = 25;
        private const int ARMY1_X_POSITION = -20;
        private const int ARMY2_X_POSITION = 20;
        private const float X_DELTA  = 1.2f;
        private const float Z_DELTA  = 1.2f;
        private bool isSpawned;
        private EntityCommandBuffer ecb;
        
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<CubeSpawner>();
        }
        
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var playerSpawnerComponent = SystemAPI.GetSingleton<CubeSpawner>();
            ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);
            
            if (!isSpawned)
            {
                CreateArmy(playerSpawnerComponent.cube1Prefab, ARMY_AMMOUNT, ARMY1_X_POSITION);
                CreateArmy(playerSpawnerComponent.cube2Prefab,ARMY_AMMOUNT,ARMY2_X_POSITION);
                isSpawned = true;
            }
        }

        private void CreateArmy(Entity prefab, int armyAmmount, int army1XPosition)
        {
            new JobInstantiator()
            {
                ecb = ecb.AsParallelWriter(),
                prefab = prefab,
                XPos = army1XPosition
            }.Schedule(armyAmmount,64)
                .Complete();
        }

        private struct JobInstantiator : IJobParallelFor
        {
            public EntityCommandBuffer.ParallelWriter ecb;
            public Entity prefab;
            public float XPos;

            private float positionX;
            private float positionZ;

            public void Execute(int index)
            {
                var entity = ecb.Instantiate(index, prefab);
                
                positionX = XPos + (index / LINE_LENGTH) * X_DELTA ;
                positionZ = (index % LINE_LENGTH) * Z_DELTA ;

                ecb.SetComponent(index,entity,new LocalTransform()
                {
                    Position = new float3(positionX,0,positionZ),
                    Scale = 1
                });
            }
        }
    }
}