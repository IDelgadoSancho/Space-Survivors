using UnityEngine;

public class EnemyController : MonoBehaviour
{

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float damage;
    [SerializeField] private float health;
    [SerializeField] private GameObject destroyEffect;

    private Vector3 direction;

    void FixedUpdate()
    {
        if (PlayerController.Instance.gameObject.activeSelf == true)
        {
            // girar hacia el player
            if (PlayerController.Instance.transform.position.x > transform.position.x)
            {
                spriteRenderer.flipX = true;
            }
            else
            {
                spriteRenderer.flipX = false;
            }

            // moverse hacia player
            direction = (PlayerController.Instance.transform.position - transform.position).normalized;
            rb.linearVelocity = new Vector2(direction.x * moveSpeed, direction.y * moveSpeed);

        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }

    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController.Instance.takeDamage(damage);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
            Instantiate(destroyEffect, transform.position, transform.rotation);
        }
    }

}
