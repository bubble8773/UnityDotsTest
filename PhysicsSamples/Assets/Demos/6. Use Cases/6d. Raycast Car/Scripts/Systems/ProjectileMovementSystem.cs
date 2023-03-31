using Unity.Burst;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;

[BurstCompile]
public partial struct ProjectileMovementSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        float deltaTime = SystemAPI.Time.DeltaTime;
        new MoveForwardJob
        {
            j_deltaTime = deltaTime
        }.Run();
    }
}

[BurstCompile]
public partial struct MoveForwardJob : IJobEntity
{
    public float j_deltaTime;

    [BurstCompile]
    public void Execute(ProjectileAspects projectileAspects)
    {
        projectileAspects.MoveForward(j_deltaTime);
    }
}
