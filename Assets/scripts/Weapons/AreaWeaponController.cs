using UnityEngine;

public class AreaWeaponController : MonoBehaviour
{
    public AreaWeapon weapon;
    private Vector3 targetSize;
    private float timer;

    void Start()
    {
        weapon = GameObject.Find("AreaWeapon").GetComponent<AreaWeapon>();
        // Destroy(gameObject, weapon.duration);
        targetSize = Vector3.one * weapon.range;
        transform.localScale = Vector3.zero;
        timer = weapon.duration;
    }

    void Update()
    {
        //grow
        transform.localScale = Vector3.MoveTowards(transform.localScale, targetSize, Time.deltaTime * 5);
        //shrink
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            targetSize = Vector3.zero;
            if (transform.localScale.x == 0f)
            {
                Destroy(gameObject, weapon.duration);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            EnemyController enemy = collider.GetComponent<EnemyController>();
            enemy.TakeDamage(weapon.damage);
        }
    }

}
