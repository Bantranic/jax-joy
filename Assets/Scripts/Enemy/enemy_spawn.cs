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
    private bool isChild = false;

    public List<GameObject> enemies;
    public string tagCheck = "Enemy";
    // Update is called once per frame
    //private GameObject parentObject;

    private void Start()
    {
        enemies = new List<GameObject>();

        
    }
    void Update()
    {
        if(Time.time >= nextSpawnTime && spawnCount < spawnAmount) 
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + 1f / spawnRate;
            spawnCount += 1;
        }

        if(spawnCount == spawnAmount) 
        {
            

            if(isChild == false)
            {
                foreach (Transform child in this.transform)
                {
                    enemies.Add(child.gameObject);
                    isChild = true;
                }


            }

            if(isChild == true) 
            {
                foreach (GameObject obj in enemies)
                {
                    if (obj == null)
                    {
                        enemies.Remove(obj);
                    }


                    if (obj != null && obj.CompareTag(tagCheck))
                    {
                        //Debug.Log(obj.name + " has the tag " + tagCheck);

                    }


                }

                //if there is no enemies in list then wave cleared
                if (enemies.Count == 0)
                {
                    Debug.Log("ENEMIES CLEARED");
                    this.enabled = false;
                }
            }
           



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
        randomPosition.y = transform.position.y;
        var spawn = Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
        spawn.transform.parent= transform;

    
    }

}
