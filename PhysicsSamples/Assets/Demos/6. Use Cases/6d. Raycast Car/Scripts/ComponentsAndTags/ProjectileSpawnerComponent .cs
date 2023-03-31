using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public struct ProjectileSpawnerComponent : IComponentData
{
    public Entity projectilePrefab;
}
