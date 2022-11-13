using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behavior/Alignment")]
public class AlignmentBehavior : FlockBehavior
{
    // Start is called before the first frame update
    public override Vector3 CalculateMove(StickAgent agent, List<Transform> context, Flock flock)
    {
        //Maintain current alignment
        if (context.Count == 0)
            return agent.transform.forward;

        //When neighbors are present move away
        Vector3 alignmentMove = Vector3.zero;
        foreach (Transform item in context)
        {
            alignmentMove += item.transform.forward;
        }
        alignmentMove /= context.Count;

        return alignmentMove;
    }
}
