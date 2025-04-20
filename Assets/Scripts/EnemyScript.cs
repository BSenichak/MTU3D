using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    NavMeshAgent agent;
    [SerializeField] int health = 100;
    [SerializeField] float speed = 1f;
    [SerializeField] GameObject blode, bonus, explosion;
    
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
    }

    private void Update()
    {
        agent.destination = GameObject.Find("Player").transform.position;
    }

    public void GetDamage(Vector3 position, int damage)
    {
        if (health > 0)
        {
            health -= damage;
            GameObject temp = Instantiate(blode, position, Quaternion.identity);
            temp.transform.parent = transform;
            Destroy(temp, 3f);
            StartCoroutine(BeRed());
        }else
        {
            FindAnyObjectByType<EnemySpawner>().SpawnSpeed *= 0.9f;
            Instantiate(explosion, transform.position, Quaternion.identity);
            FindFirstObjectByType<ManagerOfGame>().score++;
            if (Random.Range(0, 100) < 20) Instantiate(bonus, transform.position, Quaternion.identity);
            Destroy(gameObject);

        }
    }

    void OnCollisionEnter(Collision collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            FindFirstObjectByType<ManagerOfGame>().health--;
            Destroy(gameObject);
        }
    }

    IEnumerator BeRed()
    {
        GetComponent<MeshRenderer>().material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        GetComponent<MeshRenderer>().material.color = Color.white;
    }
}
