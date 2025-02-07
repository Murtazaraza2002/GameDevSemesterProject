using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    public GameObject ZombiePrefab;
    public TextMeshProUGUI LevelText;

    private int enemyCount;
    public int levelNumber = 0;
    private float spawnRangeMin = 0f;
    private float spawnRangeMax = 25f;
    private float NextSpawn = 0f;
    private float NextLevel = 30f;
    private float NextSpawnDelay = 2f;

    // Start is called before the first frame update
    void Start()
    {
        levelNumber = 1;
        SetLevelText();
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time>=NextLevel)
        {
            levelNumber++;
            SetLevelText();
            NextLevel = Time.time + 20f;
        }
        enemyCount = FindObjectsOfType<Zombie>().Length;
        if (enemyCount >= 5 * levelNumber)
            return;
        else
        {
            if(Time.time>=NextSpawn)
            {
                SpawnZombie();
                NextSpawn = Time.time + NextSpawnDelay;
            }
        }
    }
    private void SpawnZombie()
    {
        float spawnPosX = Random.Range(spawnRangeMin, spawnRangeMax);
        float spawnPosZ = Random.Range(spawnRangeMin, spawnRangeMax);
        Vector3 randomPos = new Vector3(spawnPosX, 2, spawnPosZ);
        Instantiate(ZombiePrefab, randomPos, ZombiePrefab.transform.rotation);

    }
    void SetLevelText()
    {
        LevelText.text = "Level:" + levelNumber.ToString();
    }
}
