using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateAfter(typeof(PhysicsSystemGroup))]
[BurstCompile]
public partial struct AOETriggerSystem : ISystem
{
    // ComponentLookup<LocalTransform> positionLookup;
    ComponentLookup<AOEEffect> AOELookup;
    ComponentLookup<StatsComponent> healthLookup;
    public void OnCreate(ref SystemState state)
    {
        //positionLookup = SystemAPI.GetComponentLookup<LocalTransform>(true);
        AOELookup = SystemAPI.GetComponentLookup<AOEEffect>(false);
        healthLookup = SystemAPI.GetComponentLookup<StatsComponent>(false);
    }

    public void OnDestroy(ref SystemState state)
    {
    }

    public void OnUpdate(ref SystemState state)
    {
        SimulationSingleton simulation = SystemAPI.GetSingleton<SimulationSingleton>();
        var ecbBOS = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
        //PhysicsWorldSingleton physicsWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>();
        healthLookup.Update(ref state);
        AOELookup.Update(ref state);

        state.Dependency = new AOETriggger()
        {
            AOEEffects = AOELookup,
            EnemiesHealth = healthLookup,
            ECB = ecbBOS
        }.Schedule(simulation, state.Dependency);
    }
}

[BurstCompile]
public struct AOETriggger : ITriggerEventsJob
{
    // [ReadOnly] public ComponentLookup<LocalTransform> Positions;
    public ComponentLookup<AOEEffect> AOEEffects;
    public ComponentLookup<StatsComponent> EnemiesHealth;

    public EntityCommandBuffer ECB;

    public void Execute(TriggerEvent triggerEvent)
    {
        Entity projectile = Entity.Null;
        Entity enemy = Entity.Null;

        // Identiy which entity is which
        if (AOEEffects.HasComponent(triggerEvent.EntityA))
            projectile = triggerEvent.EntityA;
        if (AOEEffects.HasComponent(triggerEvent.EntityB))
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

        // Entity impactEntity = ECB.Instantiate(AOEEffects[projectile].AOEPrefab);
        //ECB.SetComponent(impactEntity,
        //    LocalTransform.FromPosition(AOEEffects[projectile].transform));
    }
}
