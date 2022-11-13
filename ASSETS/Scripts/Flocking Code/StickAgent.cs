using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]

public class StickAgent : MonoBehaviour
{
    Flock agentFlock;
    public Flock AgentFlock { get { return agentFlock; } }

    public Queue<Vector3> AgentWayPoints = new Queue<Vector3>();
    public Vector3 target;
    Collider agentCollider;
    public Collider AgentCollider { get { return agentCollider; } }


    public void AddWaypoint(Vector3 wayPoint)
    {
        AgentWayPoints.Enqueue(wayPoint);
        Debug.Log("added waypoint " + wayPoint);
    }

    public void ClearWaypoints()
    {
        AgentWayPoints.Clear();
    }

    // Start is called before the first frame update
    void Start()
    {
        agentCollider = GetComponent<Collider>();
    }

    public void Initialize(Flock flock)
    {
        agentFlock = flock;
    }

    // Update is called once per frame
    public void Move(Vector3 velocity, Transform towerTarget, float speedMult)
    {
        /*
        if (AgentWayPoints.Count == 0)
            target = towerTarget.position;

        else if (target == towerTarget.position && AgentWayPoints.Count > 0)
            target = AgentWayPoints.Peek();

        else if (Vector3.Distance(transform.position, target) < 1 && AgentWayPoints.Count > 0)
            target = AgentWayPoints.Dequeue();
        */
        if (AgentWayPoints.Count > 0)
        {
            target = AgentWayPoints.Peek();
            if (Vector3.Distance(transform.position, target) < 0.3f) { 
                AgentWayPoints.Dequeue();
            }
        }
        if (AgentWayPoints.Count == 0)
            target = towerTarget.position;

        Vector3 dir = target - this.transform.position;
        //Debug.Log(this.transform.position);
        Vector3 normalizeDir = dir.normalized;
        velocity.y = 0;
        normalizeDir.y = 0;

        //transform.forward = velocity;
        transform.position += (velocity * Time.deltaTime * speedMult);
        transform.position += (normalizeDir * Time.deltaTime * speedMult);
        //Debug.Log("vector size: " + AgentWayPoints.Count);

        
    }
}
