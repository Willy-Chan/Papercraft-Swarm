using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMoveToTarget : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed;
    public Transform target;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = target.transform.position - this.transform.position;
        Vector3 normalizeDir = dir.normalized;
        this.transform.position += normalizeDir * speed;
    }
}
