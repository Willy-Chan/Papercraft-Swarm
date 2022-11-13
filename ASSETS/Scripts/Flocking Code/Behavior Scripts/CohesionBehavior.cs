using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Cohesion")]

public class CohesionBehavior : FlockBehavior
{
    public override Vector3 CalculateMove(StickAgent agent, List<Transform> context, Flock flock)
    {
        //When no neighbors movement vector will not be changed
        if (context.Count == 0)
            return Vector3.zero;

        //When neighbors are present move away
        Vector3 cohesionMove = Vector3.zero;
        foreach (Transform item in context)
        {
            cohesionMove += item.position;
        }
        cohesionMove /= context.Count;

        //offset from agents
        cohesionMove -= agent.transform.position;
        return cohesionMove;
    }
}
