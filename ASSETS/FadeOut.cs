using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour
{
    // Start is called before the first frame update

    public Text[] ts;
    float a;
    void Start()
    {
        a = 1f;
        IEnumerator fade = FadeOutEnum();
        StartCoroutine(fade);
    }

    public IEnumerator FadeOutEnum()
    {
        yield return new WaitForSeconds(5f);
        while (a > 0.1f)
        {
            foreach (Text t in ts)
            {
                t.color = new Color(t.color.r, t.color.g, t.color.b, a);
            }
            yield return new WaitForSeconds(0.05f);
            a -= 0.05f;
        }
        foreach (Text t in ts)
        {
            t.gameObject.SetActive(false);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
