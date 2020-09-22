using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefab;
    public int initialSpawnNo = 10;

    public Transform spawnPos;

    // Start is called before the first frame update
    void Start()
    {
        for (var i = 0; i < initialSpawnNo; i++)
        {
            Spawn();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Spawn();
        }
    }

    private void Spawn()
    {
        var randVector = Random.onUnitSphere;
        randVector.y = 0;
        Instantiate(prefab, spawnPos.position + randVector * Random.Range(1, 12), Quaternion.identity);
    }
}