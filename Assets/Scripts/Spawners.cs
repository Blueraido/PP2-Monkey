using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawners : MonoBehaviour
{
    [SerializeField] GameObject[] objectsToSpawn;
    [SerializeField] int numToSpawn;
    [SerializeField] float spawnTimer;
    [SerializeField] Transform[] spawnPos;

    int spawnCount;

    bool isSpawning;
    bool startSpawning;


    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.updateGoalEnemy(numToSpawn);
    }

    // Update is called once per frame
    void Update()
    {
        if(startSpawning && spawnCount < numToSpawn && !isSpawning) 
        {
            StartCoroutine(spawn());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            startSpawning = true;
        }
    }

    IEnumerator spawn()
    {
        isSpawning = true;
        int transformArrayPosition = Random.Range(0,spawnPos.Length);
        int objectArrayPosition = Random.Range(0,objectsToSpawn.Length);
        Instantiate(objectsToSpawn[objectArrayPosition], spawnPos[transformArrayPosition].position, spawnPos[transformArrayPosition].rotation);
        spawnCount++;
        yield return new WaitForSeconds(spawnTimer);
        isSpawning = false;
    }
}
