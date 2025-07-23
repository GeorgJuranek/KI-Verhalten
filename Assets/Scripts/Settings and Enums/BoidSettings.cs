using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BoidSettings")]
public class BoidSettings : ScriptableObject
{
    [SerializeField]
    float alignment, cohesion, seperation, target;

    public float Alignment => alignment;

    public float Cohesion => cohesion;

    public float Seperation => seperation;

    public float Target => target;


}
