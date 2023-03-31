using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public readonly partial struct MovingAspect : IAspect
{
    private readonly Entity entity;

    private readonly TransformAspect transformAspect;
    private readonly RefRO<CharectorData> charectorData;
    private readonly RefRW<TargetPosition> targetPosition;


    public void Move(float deltaTime)
    {
        float3 direction = math.normalize(targetPosition.ValueRW.value - transformAspect.Position);
        transformAspect.TranslateWorld(direction * deltaTime * charectorData.ValueRO.walkSpeed);
    }

    public void TestReachedTargetPosition(RefRW<RandomComponent> randomComponent)
    {
        float reachedTargetDistance = .5f;
        if (math.distance(transformAspect.Position, targetPosition.ValueRW.value) < reachedTargetDistance)
        {
            // Generate new random target position
            targetPosition.ValueRW.value = GetRandomPosition(randomComponent);
        }
    }

    private float3 GetRandomPosition(RefRW<RandomComponent> randomComponent)
    {
        return new float3(
            randomComponent.ValueRW.random.NextFloat(0f, 300f),
            0,
            randomComponent.ValueRW.random.NextFloat(0f, 300f)
        );
    }
}
