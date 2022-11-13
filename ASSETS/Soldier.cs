using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : Attackable
{
    public int damage;
    public float maxSpeed;
    private float currSpeed;
    public float attackRate;
    public float manaReward;
    public float awarenessRadius;
    public Transform target;

    private Animator animator;
    public string state;
    public enum AttackStyle { Melee, Ranged }
    public bool isEnemy;

    private SphereCollider awarenessCollider;

    public Attackable currEnemy;

    public GameObject indicator;

    public StickAgent stickAgent;

    private Flock flock;

    public GameObject projectile;

    public AudioClip spawn;
    public AudioClip attack;
    public AudioSource source;

    // Start is called before the first frame update
    public void Start()
    {
        base.Start();
        source = this.GetComponent<AudioSource>();
        source.clip = spawn;
        source.Play();
        if (isEnemy)
        {
            this.target = GameObject.Find("PlayerTower").transform;
            this.flock = GameObject.Find("EnemyFlock").GetComponent<Flock>();
        } else
        {
            this.target = GameObject.Find("EnemyTower").transform;
            this.flock = GameObject.Find("PlayerFlock").GetComponent<Flock>();
        }

        awarenessCollider = this.gameObject.AddComponent<SphereCollider>();
        awarenessCollider.radius = awarenessRadius;
        awarenessCollider.isTrigger = true;
        Rigidbody rb = this.gameObject.AddComponent<Rigidbody>();
        rb.isKinematic = true;
        awarenessCollider.center = new Vector3(0, -1.7f, 0);
        animator = this.GetComponentInChildren<Animator>();
        this.state = "walk";
        stickAgent = this.gameObject.AddComponent<StickAgent>();
        stickAgent.Initialize(this.flock);
        flock.agents.Add(stickAgent);
    }

    public void OnTriggerEnter(Collider other)
    {
        //Debug.Log("collision");
        if (state != "attack")
        {
            if ((this.isEnemy && other.tag == "Player") || (!this.isEnemy && other.tag == "Enemy"))
            {
                if (Vector3.Distance(this.transform.position, other.gameObject.transform.position) <= awarenessRadius)
                {
                    currEnemy = other.GetComponent<Soldier>();
                    InvokeRepeating("Attack", 0f, attackRate);
                }


            }
            if ((this.isEnemy && other.tag == "PlayerTower") || (!this.isEnemy && other.tag == "EnemyTower"))
            {
                if (Vector3.Distance(this.transform.position, other.gameObject.transform.position) <= awarenessRadius)
                {
                    currEnemy = other.GetComponent<Tower>();
                    InvokeRepeating("Attack", 0f, attackRate);
                }
            }
        }
        if ((this.isEnemy && other.tag == "PlayerProjectile") || (!this.isEnemy && other.tag == "EnemyProjectile"))
        {
            Projectile p = other.GetComponent<Projectile>();
            this.health -= p.damage;
        }
    }

    

    public void Attack()
    {
        if (this.state != "attack")
        {
            animator.Play("attack");
            source.clip = attack;
            source.Play();
            state = "attack";
        }

        if (currEnemy != null)
        {
            this.transform.right = (currEnemy.transform.position - this.transform.position).normalized;
            if (projectile == null)
            {
                if (currEnemy.health - damage <= 0)
                {
                    currEnemy.Die();
                    CancelInvoke();
                }
                else
                {
                    if (!currEnemy.attackingMeList.Contains(this))
                        currEnemy.attackingMeList.Add(this);
                    currEnemy.health -= damage;
                }
            } else
            {
                Projectile p = Instantiate(projectile, this.transform.position, this.transform.rotation).GetComponent<Projectile>();
                p.damage = damage;
                p.isEnemy = this.isEnemy;
            }
            
        }
    }


    // Update is called once per frame
    public void Update()
    {
        if (state.Equals("attack"))
        {
            this.GetComponent<Billboard>().enabled = false;
            if (this.name.Contains("Tank"))
            {
                this.transform.Find("Sprites").localScale = Vector3.one * 1.5f;
                this.transform.Find("Sprites").localPosition = new Vector3(-0.72f, -0.21f, 0.02f);
            }
        }
        if (state.Equals("attack") && currEnemy == null)
        {
            
            animator.Play("walk");
            this.state = "walk";
            CancelInvoke();
        }

        if (this.state.Equals("walk"))
        {
            if (this.name.Contains("Tank"))
            {
                this.transform.Find("Sprites").localScale = Vector3.one * 1.3f;
                this.transform.Find("Sprites").localPosition = new Vector3(-0.72f, 0.1f, 0.02f);
            }
            this.GetComponent<Billboard>().enabled = true;

            //Vector3 dir = target.transform.position - this.transform.position;
            //dir.y = 0;
            //Vector3 normalizeDir = dir.normalized;
            //this.transform.position += normalizeDir * maxSpeed;

            List<Transform> context = flock.NearbyObj(stickAgent);
            Vector3 move = flock.behavior.CalculateMove(stickAgent, context, flock);
            //above line gets avoidance behavior
            //need to calculate vector towards target here
            // add that to move
            // specific for each agent -- maybe in stickagent class

            move *= flock.driveFactor;
            if (move.sqrMagnitude > flock.squareMaxSpeed)
            {
                move = move.normalized;
                move.y = 0;
            }

            stickAgent.Move(move, flock.towerTarget, maxSpeed);
            //Debug.Log(move);
        }

        if (this.health <= 0)
        {
            Die();
        }

        indicator.transform.localPosition = this.transform.localPosition;
    }
}
