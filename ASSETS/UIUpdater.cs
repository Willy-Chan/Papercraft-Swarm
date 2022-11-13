using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIUpdater : MonoBehaviour
{
    public Text killText;
    public int killCount = 0;
    public Text deathText;
    public int deathCount = 0;
    public Text fortText;
    public int fortCount = 10;
    public RectTransform healthBar;
    public int healthCount = 1500;
    public RectTransform enemyHealthBar;
    public int enemyHealthCount = 1500;
    public RectTransform manaBar;
    public int manaCount = 250;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        killText.text = ": " + killCount;
        deathText.text = ": " + deathCount;
        fortText.text = ": " + fortCount;
        healthBar.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, healthCount/3f);
        enemyHealthBar.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, enemyHealthCount/3f);
        manaBar.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, manaCount);
    }
}
