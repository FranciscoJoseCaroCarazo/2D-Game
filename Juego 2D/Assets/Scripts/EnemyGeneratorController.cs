using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGeneratorController : MonoBehaviour
{

    public GameObject enemyPrefab;
    public float generatorTimer = 3.25f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateEnemy()
    {
        Instantiate(enemyPrefab, transform.position, Quaternion.identity);
    }

    public void StartGeneration()
    {
        InvokeRepeating("CreateEnemy", 0f, generatorTimer);
    }

    public void StopGeneration(bool clean = false)
    {
        CancelInvoke("CreateEnemy");
        if (clean)
        {
            Object[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in allEnemies)
            {
                Destroy(enemy);
            }
        }
    }
}
