using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class StatsComponentAurthoring : MonoBehaviour
{
    public float MaxValue;
    public float CurrentValue;
}

public class StatsComponentBaker : Baker<StatsComponentAurthoring>
{
    public override void Bake(StatsComponentAurthoring authoring)
    {
        AddComponent(new StatsComponent
        {
            MaxValue = authoring.MaxValue,
            CurrentValue = authoring.CurrentValue
        });
    }
}
