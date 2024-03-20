using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_spawn : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float spawnRate = 10f;
    public float spawnAreaRadius = 5f;
    private float nextSpawnTime;
    public  float spawnAmount;
    // Update is called once per frame
    void Update()
    {
        if(Time.time >= nextSpawnTime) 
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + 1f / spawnRate;
        }
        
    }

    void SpawnEnemy() 
    {
        Vector3 randomPosition = transform.position + Random.insideUnitSphere * spawnAreaRadius;
        randomPosition.y = 0;
        Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
    
    }
}
