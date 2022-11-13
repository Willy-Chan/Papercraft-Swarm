using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public Transform towerTarget;
    public StickAgent agentPrefab;
    Queue<Vector3> wayPoints = new Queue<Vector3>();
    public List<StickAgent> agents = new List<StickAgent>();
    public FlockBehavior behavior;
    
    [Range(10, 500)]
    const float AgentDensity = .08f;

    [Range(0f, 100f)]
    public float driveFactor = 10f;

    [Range(0f, 100f)]
    public float maxSpeed = 5f;

    [Range(0f, 10f)]
    public float neighborRadius = 1.5f;

    [Range(0f, 10f)]
    public float avoidanceRadiusMult = 0.9f;

    public float squareMaxSpeed;
    float squareNeighborRaius;
    float squareAvoidanceRadius;
    public float SquareAvoidanceRadius { get { return squareAvoidanceRadius; } }
    public int count = 0;

    // Start is called before the first frame update
    void Start()
    {
        squareMaxSpeed = maxSpeed * maxSpeed;
        squareNeighborRaius = neighborRadius* neighborRadius;
        squareAvoidanceRadius = squareNeighborRaius * avoidanceRadiusMult * avoidanceRadiusMult;

        if(towerTarget == null)
            newTarget();
    }

    public void NewAgent()
    {
        Vector3 pos1 = Random.insideUnitSphere * 0.2f;
        pos1.y = 0;
        StickAgent newAgent = Instantiate(
            agentPrefab,
            pos1,
            Quaternion.Euler(Vector3.forward),
            transform
            )  ;
        newAgent.name = "Agent" + count;
        newAgent.Initialize(this);
        count++;
        agents.Add(newAgent);
        newAgent.target = towerTarget.position;
    }

    public void newTarget() {
            float xPos = Random.Range(-10f, 10f);
            float zPos = Random.Range(-10f, 10f);
            GameObject g = new GameObject();
            towerTarget = g.transform;
            towerTarget.position = new Vector3(xPos, 0, zPos);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)){
            NewAgent();
        }
        
        if (Input.GetKeyDown(KeyCode.P))
        {
            for (int i = 0; i < 6; i++)
            {
                float xPos = Random.Range(-10f, 10f);
                float zPos = Random.Range(-10f, 10f);
                GameObject f = new GameObject();
                f.transform.position = new Vector3(xPos, 0, zPos);
                wayPoints.Enqueue(f.transform.position);
                //Debug.Log(wayPoints.Count);
            }

            foreach (StickAgent agent in agents)
            {
                Queue<Vector3> temp = new Queue<Vector3>(wayPoints);
                agent.AgentWayPoints = temp;
                agent.target = agent.AgentWayPoints.Dequeue();
            }
        }

        /*
        foreach (StickAgent agent in agents)
        {
            List<Transform> context = NearbyObj(agent);
            Vector3 move = behavior.CalculateMove(agent, context, this);
            //above line gets avoidance behavior
            //need to calculate vector towards target here
            // add that to move
            // specific for each agent -- maybe in stickagent class

            move *= driveFactor;
            if (move.sqrMagnitude > squareMaxSpeed)
            {
                move = move.normalized * maxSpeed;
                move.y = 0;
            }

            agent.Move(move, towerTarget);
            //Debug.Log(move);
        }
        */
    }

    public List<Transform> NearbyObj(StickAgent agent)
    {
        List<Transform> context = new List<Transform>();
        Collider[] contextColliders = Physics.OverlapSphere(agent.transform.position, neighborRadius);
        foreach (Collider c in contextColliders)
        {
            if (c != agent.AgentCollider)
            {
                context.Add(c.transform);
            }
        }
        return context;
    }
}
