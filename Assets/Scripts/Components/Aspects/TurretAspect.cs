using Unity.Entities;

namespace Components.Aspects
{
    public readonly partial struct TurretAspect : IAspect
    {
        public readonly Entity entity;
        public Entity CannonBallSpawn => turret.ValueRO.CannonBallSpawn;
        public Entity CannonBallPrefab => turret.ValueRO.CannonBallPrefab;
        
        readonly RefRO<Turret> turret;
        
        public readonly RefRW<Timer> shootTimerComponent;
    }
}