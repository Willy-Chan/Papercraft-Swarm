using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObstacleManager : MonoBehaviour
{
    public UIUpdater uIUpdater;
    public GameObject lineSegmentsFolder;
    public GameObject lineSegment;
    public Material lineMaterial;
    public bool drawing;
    public float minDist;
    private GameObject controller;
    public GameObject leftController;
    public GameObject rightController;
    public GameObject pointerSphere;
    private GameObject currLine;
    private LineRenderer currLineRenderer;
    public OVRInput.RawButton leftButtonPointer;
    public OVRInput.RawButton rightButtonPointer;
    public OVRInput.RawButton leftButtonDraw;
    public OVRInput.RawButton rightButtonDraw;
    public OVRInput.RawButton leftButtonCancel;
    public OVRInput.RawButton rightButtonCancel;
    private LineRenderer pointerRenderer;
    private bool drawRight;
    private bool drawLeft;
    public GameObject barricadeUnit;
    private bool lineDrawn;
    public Transform arena;
    public bool readyToPaint = false;
    public Button b;
    public Text t;
    List<GameObject> inside;
    public bool selecting;
    public Material selectedMat;
    public Material origMat;

    // Start is called before the first frame update
    void Start()
    {
        pointerRenderer = GetComponent<LineRenderer>();
        uIUpdater = FindObjectOfType<UIUpdater>();
        inside = new List<GameObject>();

    }


    public void ClickButton(Button b)
    {
        IEnumerator clickEnum = ClickEnum(b);
        StartCoroutine(clickEnum);
    }

    public IEnumerator ClickEnum(Button b)
    {
        Vector3 startScale = b.transform.localScale;
        b.transform.localScale = startScale * 0.9f;
        yield return new WaitForSeconds(0.1f);
        b.transform.localScale = startScale;

    }

    public void ReadyToPaint()
    {
        readyToPaint = !readyToPaint;
    }

    // Update is called once per frame
    void Update()
    {
        bool leftPress = false;
        bool rightPress = false;


        if (uIUpdater.fortCount == 0)
        {
            b.interactable = false;
        }

        if (readyToPaint)
        {
            t.color = Color.red;
        } else
        {
            t.color = Color.black;
        }
        if ((!lineDrawn && uIUpdater.fortCount > 0) || selecting)
        {
            if (OVRInput.Get(leftButtonPointer))
            {
                controller = leftController;
                leftPress = true;
            }
            else
            {
                if (OVRInput.Get(rightButtonPointer))
                {
                    controller = rightController;
                    rightPress = true;
                }
            }
        }

        if (lineDrawn)
        {
            if (OVRInput.GetDown(leftButtonDraw) || OVRInput.GetDown(rightButtonDraw)){
                if (readyToPaint)
                {
                    BuildBarricade();
                    readyToPaint = false;
                } else
                {
                    
                }
            } else if (OVRInput.GetDown(leftButtonCancel) || OVRInput.GetDown(rightButtonCancel))
            {
                Destroy(currLine);
                lineDrawn = false;
            }


        }

        if ((leftPress || rightPress))
        {
            
            Vector3 pos = RaycastBeam();
            if (!drawing)
            {
                if (!selecting)
                {
                    if ((OVRInput.GetDown(leftButtonDraw) && leftPress) || (OVRInput.GetDown(rightButtonDraw) && rightPress))
                    {
                        drawing = true;
                        if (leftPress) drawLeft = true;
                        if (rightPress) drawRight = true;
                        if (readyToPaint)
                            DrawLine(Color.black);
                        else
                        {
                            DrawLine(Color.yellow);
                        }

                    }
                } else
                {
                    if ((OVRInput.GetDown(leftButtonDraw) && leftPress) || (OVRInput.GetDown(rightButtonDraw) && rightPress))
                    {
                        foreach (GameObject g in inside)
                        {
                            g.GetComponent<StickAgent>().AddWaypoint(pos);
                            g.transform.Find("Indicator").GetComponent<MeshRenderer>().material = origMat;
                        }
                        inside.Clear();
                        selecting = false;
                        lineDrawn = false;
                    }
                }
            }
        } else
        {
            pointerSphere.SetActive(false);
            pointerRenderer.enabled = false;
        }
        if (OVRInput.GetUp(rightButtonDraw) && drawRight || OVRInput.GetUp(OVRInput.Button.Left) && drawLeft)
        {
            drawLeft = false;
            drawRight = false;
            drawing = false;
        }
    }

    public void BuildBarricade()
    {
        IEnumerator builder = BarricadeBuilder();
        StartCoroutine(builder);
    }

    public IEnumerator BarricadeBuilder()
    {
        for (int i = 0; i < currLineRenderer.positionCount; i+=2)
        {
            GameObject b = Instantiate(barricadeUnit);
            b.transform.position = currLineRenderer.GetPosition(i);
            int inside = i + 1;
            if (inside > currLineRenderer.positionCount) inside = i - 1;
            Vector3 rot = (currLineRenderer.GetPosition(inside) - b.transform.position + new Vector3(20, 0, 0)).normalized;
            b.transform.right = rot;
            b.transform.SetParent(arena, true);
            uIUpdater.fortCount -= 1;
            if (uIUpdater.fortCount <= 0)
            {
                uIUpdater.fortCount = 0;
                break;
            }
            yield return new WaitForSeconds(0.1f);
        }
        lineDrawn = false;
        Destroy(currLine);
    }

    public void DrawLine(Color color)
    {
        if (lineSegmentsFolder == null)
            lineSegmentsFolder = new GameObject("line segment folder");
        Debug.Log("callign coroutine");
        IEnumerator brushRoutine = BrushPreviewRoutine(color);
        StartCoroutine(brushRoutine);

    }

    public Vector3 RaycastBeam()
    {
        
        int layerMask = 1 << 7;
        RaycastHit hit;
        //Debug.Log("raycasting");
        pointerRenderer.enabled = true;
        if (Physics.Raycast(controller.transform.position, controller.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(controller.transform.position, controller.transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            //Debug.Log("Did Hit");

            var distance = Vector3.Distance(pointerRenderer.GetPosition(0), pointerRenderer.GetPosition(1));
            // Scale the line's texture depending on the distance
            pointerRenderer.materials[0].mainTextureScale = new Vector3(distance * 5f, 1, 1);
            pointerRenderer.SetPosition(0, pointerRenderer.transform.InverseTransformPoint(controller.transform.position));
            pointerRenderer.SetPosition(1, pointerRenderer.transform.InverseTransformPoint(hit.point));
            pointerSphere.transform.position = hit.point;
            pointerSphere.SetActive(true);
            pointerRenderer.materials[0].SetTextureOffset("_BaseMap", new Vector2(-0.05f, 0f) + pointerRenderer.materials[0].GetTextureOffset("_BaseMap"));

            return hit.point;

        }
        else
        {
            Debug.DrawRay(controller.transform.position, controller.transform.TransformDirection(Vector3.forward) * 1000, Color.white);
        }
        
        return Vector3.one * -1f;
    }

    IEnumerator BrushPreviewRoutine(Color lineColor)
    {
        bool first = false;
        Debug.Log("starting coroutine");
        currLine = Instantiate(lineSegment, pointerSphere.transform, false);
        currLineRenderer = currLine.GetComponent<LineRenderer>();
        currLineRenderer.startColor = lineColor;
        currLineRenderer.endColor = lineColor;
        currLineRenderer.material = new Material(lineMaterial);
        currLineRenderer.material.color = lineColor;
        currLine.transform.localPosition = Vector3.zero;
        currLine.transform.SetParent(lineSegmentsFolder.transform, true);
        currLineRenderer.SetPosition(0, currLine.transform.position);
        while (drawing)
        {
            //Debug.Log("in while");
            currLine.transform.position = pointerSphere.transform.position;
            float dist = Vector3.Distance(currLine.transform.position, currLineRenderer.GetPosition(currLineRenderer.positionCount - 1));
            if (dist > minDist)
            {
                currLineRenderer.positionCount++;
                currLineRenderer.SetPosition(currLineRenderer.positionCount - 1, currLine.transform.position);
            } else
            {
                //Debug.Log("not far enough");
                Debug.Log(pointerSphere.transform.position);
            }
            yield return null;

        }
        lineDrawn = true;

        if (!readyToPaint)
        {
            selecting = true;
            Vector3[] positions = new Vector3[currLineRenderer.positionCount];
            currLineRenderer.GetPositions(positions);
            GameObject[] s = GameObject.FindGameObjectsWithTag("Player");
            
            foreach (GameObject go in s)
            {
                if (SelectInsideShape.checkInside(positions, currLineRenderer.positionCount, go.transform.position))
                {
                    inside.Add(go);
                    go.transform.Find("Indicator").GetComponent<MeshRenderer>().material = selectedMat;
                }
            }
            Destroy(currLine);
            
        }
    }
}
