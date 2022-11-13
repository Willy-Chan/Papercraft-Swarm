using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : Attackable
{
    // Start is called before the first frame update    

    

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (this.tag == "PlayerTower")
        {
            uIUpdater.healthCount = this.health;
        }
        else if (this.tag == "EnemyTower")
        {
            uIUpdater.enemyHealthCount = this.health;
        }
    }
}
