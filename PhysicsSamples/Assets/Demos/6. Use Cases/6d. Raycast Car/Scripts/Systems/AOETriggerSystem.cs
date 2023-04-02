using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using UnityEngine;

[BurstCompile]
public partial struct AOETriggerSystem : ISystem
{
    ComponentLookup<LifeTime> impactLookup;
    ComponentLookup<StatsComponent> healthLookup;
    public void OnCreate(ref SystemState state)
    {
        impactLookup = SystemAPI.GetComponentLookup<LifeTime>(false);
        healthLookup = SystemAPI.GetComponentLookup<StatsComponent>(false);
    }

    public void OnDestroy(ref SystemState state)
    {
    }

    public void OnUpdate(ref SystemState state)
    {
        SimulationSingleton simulation = SystemAPI.GetSingleton<SimulationSingleton>();
        var ecbBOS = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
        healthLookup.Update(ref state);
        impactLookup.Update(ref state);

        state.Dependency = new AOETriggger()
        {
            Projectiles = impactLookup,
            EnemiesHealth = healthLookup,
            ECB = ecbBOS
        }.Schedule(simulation, state.Dependency);
    }
}

[BurstCompile]
public struct AOETriggger : ITriggerEventsJob
{
    //[ReadOnly] public ComponentLookup<LocalTransform> Positions;
    public ComponentLookup<LifeTime> Projectiles;
    public ComponentLookup<StatsComponent> EnemiesHealth;

    public EntityCommandBuffer ECB;

    public void Execute(TriggerEvent triggerEvent)
    {
        Entity projectile = Entity.Null;
        Entity enemy = Entity.Null;

        // Identiy which entity is which
        if (Projectiles.HasComponent(triggerEvent.EntityA))
            projectile = triggerEvent.EntityA;
        if (Projectiles.HasComponent(triggerEvent.EntityB))
            projectile = triggerEvent.EntityB;
        if (EnemiesHealth.HasComponent(triggerEvent.EntityA))
            enemy = triggerEvent.EntityA;
        if (EnemiesHealth.HasComponent(triggerEvent.EntityB))
            enemy = triggerEvent.EntityB;

        // if its a pair of entity we don't want to process, exit
        if (Entity.Null.Equals(projectile)
            || Entity.Null.Equals(enemy)) return;

        // Damage enemy
        StatsComponent currentHealth = EnemiesHealth[enemy];
        currentHealth.CurrentValue -= 2;
        EnemiesHealth[enemy] = currentHealth;
        Debug.Log(currentHealth.CurrentValue);

        // Destroy enemy if it is out of health
        if (currentHealth.CurrentValue <= 0)
            ECB.DestroyEntity(enemy);
    }
}
