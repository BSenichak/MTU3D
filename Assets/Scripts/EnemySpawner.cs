using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject Chmonya, Putler;
    public float SpawnSpeed = 5f;

    private void Start()
    {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        while (true)
        {
            GameObject bot = Random.Range(0, 100) < 20 ? Putler : Chmonya;
            Vector3 spawnPosition = transform.GetChild(Random.Range(0, 4)).position;
            Instantiate(bot, spawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(SpawnSpeed);
        }
    }
}
