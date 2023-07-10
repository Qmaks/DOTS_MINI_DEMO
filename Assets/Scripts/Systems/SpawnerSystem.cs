using Components;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Systems
{
    public partial class SpawnerSystem : SystemBase
    {
        private const int ARMY_AMMOUNT = 100;
        private const int LINE_LENGTH  = 25;
        private const int ARMY1_X_POSITION = -20;
        private const int ARMY2_X_POSITION = 20;
        private const float X_DELTA  = 1.2f;
        private const float Z_DELTA  = 1.2f;

        private bool isSpawned = false;
        
        protected override void OnUpdate()
        {
            var playerSpawnerComponent = SystemAPI.GetSingleton<CubeSpawner>();

            if (!isSpawned)
            {
                CreateArmy(playerSpawnerComponent.cube1Prefab, ARMY_AMMOUNT, ARMY1_X_POSITION);
                CreateArmy(playerSpawnerComponent.cube2Prefab,ARMY_AMMOUNT,ARMY2_X_POSITION);
                isSpawned = true;
            }
        }

        private void CreateArmy(Entity prefab,int amount,float XPos)
        {
            var entities = EntityManager.Instantiate(prefab,amount,Allocator.Temp);
            var positionX = 0f;
            var positionZ = 0f;
            var count = 0;
            foreach (var entity in entities)
            {
                var transform = SystemAPI.GetComponentRW<LocalTransform>(entity);
                positionX = XPos + (count / LINE_LENGTH) * X_DELTA ;
                positionZ = (count % LINE_LENGTH) * Z_DELTA ;
                transform.ValueRW.Position = new float3(positionX, 0, positionZ);
                count++;
            }
        }
    }
}