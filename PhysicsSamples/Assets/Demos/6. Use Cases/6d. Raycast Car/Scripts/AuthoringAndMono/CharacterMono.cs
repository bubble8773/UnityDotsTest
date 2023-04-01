using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class CharacterMono : MonoBehaviour
{
    public float walkspeed;
}

public class CharectorBaker : Baker<CharacterMono>
{
    public override void Bake(CharacterMono authoring)
    {
        AddComponent(new CharectorData { walkSpeed = authoring.walkspeed });
    }
}
