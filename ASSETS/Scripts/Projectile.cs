using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject explosion;
    public float forwardVelocity;
    public float upVelocity;
    public Rigidbody rb;
    public bool isEnemy;
    public int damage;
    void Start()
    {
        rb.AddForce(forwardVelocity * this.transform.right);
        rb.AddForce(upVelocity * this.transform.up);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isEnemy && other.tag.Contains("Player") || !isEnemy && other.tag.Contains("Enemy"))
        {
            if(explosion != null)
            {
                GameObject g = Instantiate(explosion, this.transform.position, this.transform.rotation);
                Destroy(this.gameObject);
            }
            other.GetComponent<Attackable>().health -= damage;
            Destroy(this.gameObject);

        } else if (other.tag == "Ground")
        {
            if (explosion != null)
            {
                GameObject g = Instantiate(explosion, this.transform.position, this.transform.rotation);
                Destroy(this.gameObject);
            } else
            {
                rb.isKinematic = true;
                this.GetComponent<Collider>().enabled = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.right = this.rb.velocity;
        if (this.transform.position.y < -1) Destroy(this.gameObject);
    }
}
