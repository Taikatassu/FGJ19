using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GrabbableInfo : ScriptableObject
{
    public GameObject grabbablePrefab;
    [Range(0f, 1f), Tooltip("1 = common spawn, 0 = never spawns")]
    public float rarity = 1;
}
