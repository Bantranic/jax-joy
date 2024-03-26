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
    private float spawnCount;
    // Update is called once per frame
    void Update()
    {
        if(Time.time >= nextSpawnTime && spawnCount <= spawnAmount) 
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + 1f / spawnRate;
            spawnCount += 1;
        }


        /*if (gameObject.GetComponentInChildren<GameObject>() == null) 
        {
            Debug.Log("NO object");
        
        }
        else 
        {
            Debug.Log("object" + gameObject.name);
        }*/
        
    }

    void SpawnEnemy() 
    {
        Vector3 randomPosition = gameObject.transform.position + Random.insideUnitSphere * spawnAreaRadius;
        randomPosition.y = 0;
        var spawn = Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
        spawn.transform.parent = transform;

    
    }

}
