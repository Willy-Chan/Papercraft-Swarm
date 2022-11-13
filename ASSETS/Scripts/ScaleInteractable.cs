using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScaleInteractable : MonoBehaviour
{
    public GameObject bigVersion;
    public GameObject smallVersion;
    public GameObject marker;
    private GameObject controller;

    public GameObject leftController;
    public GameObject rightController;
    public OVRInput.RawButton rightButtonPress;
    public OVRInput.RawButton leftButtonPress;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        bool press = false;
        if (OVRInput.GetDown(leftButtonPress))
        {
            controller = leftController;
            press = true;
        } else
        {
            if (OVRInput.GetDown(rightButtonPress)){
                controller = rightController;
                press = true;
            }
        }


        if (press)
        {
            int layerMask1 = 1 << 5;
            int layerMask2 = 1 << 6;
            int layerMask = layerMask1 | layerMask2;
            RaycastHit hit;
            Debug.Log("raycasting");
            if (Physics.Raycast(controller.transform.position, controller.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
            {
                Debug.DrawRay(controller.transform.position, controller.transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                Debug.Log("Did Hit");
                if (hit.collider.gameObject.layer == 6)
                {
                    GameObject mark = Instantiate(marker);
                    GameObject littleMark = Instantiate(marker);
                    littleMark.transform.position = hit.point;
                    littleMark.transform.SetParent(this.transform, true);
                    littleMark.transform.localScale = Vector3.one * 0.1f;
                    mark.transform.SetParent(bigVersion.transform);
                    mark.transform.localPosition = littleMark.transform.localPosition;
                } else
                {
                    Debug.Log("ui hit");
                    hit.collider.GetComponent<Button>().onClick.Invoke();
                }

            }
            else
            {
                Debug.DrawRay(controller.transform.position, controller.transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            }
        }
    }
}
