using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{
    public UIUpdater uIUpdater;
    // Start is called before the first frame update
    public GameObject[] playerPrefabs;
    public GameObject[] enemyPrefabs;

    public int[] costs;

    public Transform playerSpawnPoint;
    public Transform enemySpawnPoint;

    public GameObject playerFolder;
    public GameObject enemyFolder;

    public GameObject playerIndicator;
    public GameObject enemyIndicator;

    public Transform minimap;

    public int playerBalance = 250;
    public int enemyCount;

    public Transform lookTarget;

    public List<Button> buttons;

    public AudioSource playerSource;
    public AudioSource enemySource;
    void Start()
    {
        uIUpdater = FindObjectOfType<UIUpdater>();
        InvokeRepeating("RandomEnemySpawner", 10f, 5f);
        InvokeRepeating("AddToBalance", 0f, 0.2f);
    }

    public void AddToBalance()
    {
        if (playerBalance < 500)
        playerBalance++;
    }

    public void RandomEnemySpawner()
    {
        StartCoroutine("DelaySpawn");
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
    public IEnumerator DelaySpawn()
    {
        //int index = 0; //CHANGE LATER WITH RANDOM VALS
        int index = Random.Range(0, 5);
        if (index < 3) index = 0;
        else if (index < 5) index = 1;
        else index = 2;
        //Debug.Log(index);
        
        float number = Random.Range(0f, 4f);
        float delay = number / 2;
        for (int i = 0; i < number; i++)
        {
            SpawnEnemySoldier(index);
            yield return new WaitForSeconds(0.4f);
        }
        yield return new WaitForSeconds(delay);

    }

    public void SpawnPlayerSoldier(int index)
    {
        
        if (playerBalance > costs[index])
        {
            Vector3 randomOffset = Random.insideUnitSphere * 0.02f;
            randomOffset.y = 0;
            GameObject soldier = Instantiate(playerPrefabs[index], playerSpawnPoint.position + randomOffset, Quaternion.identity, playerFolder.transform);
            GameObject indicator = Instantiate(playerIndicator);
            indicator.transform.SetParent(minimap, true);
            indicator.transform.localScale *= 0.5f;
            soldier.GetComponent<Soldier>().indicator = indicator;
            soldier.GetComponent<Billboard>().camera = lookTarget;
            playerBalance -= costs[index];
            playerSource.Play();
        }
    }

    public void SpawnEnemySoldier(int index)
    {
        Vector3 randomOffset = Random.insideUnitSphere * 0.02f;
        randomOffset.y = 0;
        GameObject soldier = Instantiate(enemyPrefabs[index], enemySpawnPoint.position + randomOffset, Quaternion.identity, enemyFolder.transform);
        GameObject indicator = Instantiate(enemyIndicator);
        indicator.transform.SetParent(minimap, true);
        indicator.transform.localScale *=  0.5f;
        soldier.GetComponent<Soldier>().indicator = indicator;
        soldier.GetComponent<Billboard>().camera = lookTarget;
        enemySource.Play();
        enemyCount++;
    }

    // Update is called once per frame
    private void Update()
    {
        uIUpdater.manaCount = playerBalance;
        for (int i = 0; i < costs.Length; i++)
        {
            if (playerBalance < costs[i])
            {
                buttons[i].interactable = false;
            } else
            {
                buttons[i].interactable = true;
            }
        }
    }
}
