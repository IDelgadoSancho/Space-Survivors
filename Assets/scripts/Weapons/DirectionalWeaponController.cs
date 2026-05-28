using UnityEngine;

public class DirectionalWeaponController : MonoBehaviour
{
    public DirectionalWeapon weapon;
    private Vector3 direction;
    private float duration;
    private bool hasHit = false;

    void Start()
    {

        weapon = GameObject.Find("Laser Gun").GetComponent<DirectionalWeapon>();
        duration = weapon.stats[weapon.weaponLevel].duration;

        Transform target = FindClosestEnemy();

        if (target != null)
        {
            direction = (target.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
        else
        {
            direction = PlayerController.Instance.lastMoveDirection;

        }
    }

    void Update()
    {
        transform.position += direction * weapon.stats[weapon.weaponLevel].speed * Time.deltaTime;

        duration -= Time.deltaTime;
        if (duration <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasHit) return;

        if (collision.CompareTag("Enemy"))
        {
            hasHit = true;

            EnemyController enemy = collision.GetComponent<EnemyController>();
            enemy.TakeDamage(weapon.stats[weapon.weaponLevel].damage);

            Destroy(gameObject);
        }
    }

    private Transform FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        Transform closest = null;
        float minDistance = weapon.stats[weapon.weaponLevel].range;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);

            if (distance < minDistance)
            {
                minDistance = distance;
                closest = enemy.transform;
            }
        }

        return closest;
    }

}
