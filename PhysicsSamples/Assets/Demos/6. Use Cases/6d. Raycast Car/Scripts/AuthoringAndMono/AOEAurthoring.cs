using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class AOEAurthoring : MonoBehaviour
{
    public GameObject effect;
}

public class AOEBaker : Baker<AOEAurthoring>
{
    public override void Bake(AOEAurthoring authoring)
    {
        AddComponent(new AOEEffect
            { AOEPrefab = GetEntity(authoring.effect) });
    }
}
