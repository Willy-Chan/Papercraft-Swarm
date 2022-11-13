using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FlockBehavior : ScriptableObject
{
    public abstract Vector3 CalculateMove(StickAgent agent, List<Transform> context, Flock flock); 
}