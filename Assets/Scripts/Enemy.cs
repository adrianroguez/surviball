using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public float speed = 3.5f;
    public int health = 3;

    private Transform player;
    private Renderer enemyRenderer;
    private Color originalColor;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        enemyRenderer = GetComponent<Renderer>();
        originalColor = enemyRenderer.material.color;
    }

    void Update()
    {
        if (player != null)
        {
            transform.LookAt(player);
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
    }

    public void TakeDamage()
    {
        health--;
        StartCoroutine(FlashDamage());

        if (health <= 0)
        {
            if(GameManager.instance != null)
            {
                GameManager.instance.AddScore();
            }
            Die();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerScript = other.GetComponent<PlayerController>();
            if (playerScript != null)
            {
                playerScript.TakeDamage(1);
            }
            Die();
        }
    }

    IEnumerator FlashDamage()
    {
        enemyRenderer.material.color = Color.white;
        yield return new WaitForSeconds(0.1f);
        enemyRenderer.material.color = originalColor;
    }

    void Die()
    {
        Destroy(gameObject);
    }
}