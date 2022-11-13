using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Flock/Filter/Same Flock")]
public class SameFlock : ContextFilter
{
    public override List<Transform> Filter(StickAgent agent, List<Transform> original)
    {
        List<Transform> filtered = new List<Transform>();
        foreach(Transform item in original)
        {
            StickAgent itemAgent = item.GetComponent<StickAgent>();
            if(itemAgent != null && itemAgent.AgentFlock == agent.AgentFlock)
            {
                filtered.Add(item);
            }

        }
        return filtered;
    }


}
