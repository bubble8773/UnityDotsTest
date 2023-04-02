using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
public struct StatsComponent : IComponentData
{
    public float MaxValue;
    public float CurrentValue;
}
