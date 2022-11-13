using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    // Start is called before the first frame update

    public bool lookAtCamera;
    public Vector3 relativeLookDir;
    public Transform camera;
    public bool flip;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!lookAtCamera)
        {
            this.transform.forward = relativeLookDir;
        } else
        {
            this.transform.LookAt(camera);
            if (flip)
            {
                this.transform.forward *= -1f;
            }
        }
    }
}
