using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public readonly partial struct ProjectileAspects : IAspect
{
    private readonly Entity entity;

    private readonly TransformAspect transformAspect;
    private readonly RefRO<ProjectileMovementComponent> projectileMovement;

    public void MoveForward(float deltaTime)
    {
        float3 direction = math.normalize(transformAspect.Forward);
        transformAspect.TranslateWorld(direction * deltaTime * projectileMovement.ValueRO.speed);
    }
}
