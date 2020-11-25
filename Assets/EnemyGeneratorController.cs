using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGeneratorController : MonoBehaviour
{

    public GameObject enemyPrefab;
    public float generatorTimer=1.75f; //cada cuanto tiempo se genera un enemigo

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

    public void StartGenerator()//Empieza a crear enemigos
    {
        InvokeRepeating("CreateEnemy", 0f, generatorTimer); //Invocamos el motodo crear enemigo y le enviamos los tiempos de generacion
    }

    public void CancelGenerator(bool clean=false)
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
