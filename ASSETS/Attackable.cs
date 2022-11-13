using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attackable : MonoBehaviour
{
    // Start is called before the first frame update

    public UIUpdater uIUpdater;


    public int health;
    public List<Soldier> attackingMeList;
    public void Start()
    {
        uIUpdater = FindObjectOfType<UIUpdater>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Die()
    {
        if (this.tag.Contains("Enemy"))
        {
            uIUpdater.killCount++;
        } else
        {
            uIUpdater.deathCount++;
        }

        for (int i = 0; i < attackingMeList.Count; i++)
        {
            attackingMeList[i].currEnemy = null;
        }
        if(this.gameObject.GetComponent<Soldier> () != null)
        {
            Destroy (this.gameObject.GetComponent<Soldier> ().indicator);
        }

        Destroy(this.gameObject);
    }
}
