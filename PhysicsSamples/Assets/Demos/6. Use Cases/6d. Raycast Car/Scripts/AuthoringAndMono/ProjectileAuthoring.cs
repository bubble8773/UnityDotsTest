using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class ProjectileAuthoring : MonoBehaviour
{
    public float speed;
}

public class ProjectileBaker : Baker<ProjectileAuthoring>
{
    public override void Bake(ProjectileAuthoring authoring)
    {
        AddComponent(new ProjectileMovementComponent
        {
            speed = authoring.speed
        });
    }
}
