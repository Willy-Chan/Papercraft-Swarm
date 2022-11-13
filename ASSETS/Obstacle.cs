using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    // Start is called before the first frame update

    public Transform minimap;
    public GameObject icon;
    public Vector3 iconScale;
    public Vector3 iconOffset;
    private GameObject indicator;
    void Start()
    {
        minimap = FindObjectOfType<ScaleInteractable>().smallVersion.transform;
        indicator = Instantiate(icon);
        indicator.transform.SetParent(minimap, true);
        indicator.transform.localPosition = this.transform.localPosition + iconOffset;
        indicator.transform.localScale = iconScale;
        indicator.transform.localEulerAngles = new Vector3(90, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
