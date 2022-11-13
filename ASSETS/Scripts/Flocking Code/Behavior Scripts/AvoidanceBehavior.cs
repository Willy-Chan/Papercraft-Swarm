using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Avoidance")]
public class AvoidanceBehavior : FilteredFlockBehavior
{
    // Start is called before the first frame update
    public override Vector3 CalculateMove(StickAgent agent, List<Transform> context, Flock flock)
    {
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context);
        //When no neighbors movement vector will not be changed
        if (filteredContext.Count == 0)
            return Vector3.zero;

        //When neighbors are present move away
        int numAvoid = 0;
        Vector3 avoidanceMove = Vector3.zero;
        foreach (Transform item in filteredContext)
        {
            if (Vector3.SqrMagnitude(item.position - agent.transform.position) < flock.SquareAvoidanceRadius)
            {
                numAvoid++;
                avoidanceMove += (agent.transform.position - item.position);
            }
        }
        if(numAvoid > 0)
            avoidanceMove /= numAvoid;
        return avoidanceMove;
    }
}