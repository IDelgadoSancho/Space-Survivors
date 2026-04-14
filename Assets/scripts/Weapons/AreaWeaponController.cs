using System.Collections.Generic;
using UnityEngine;

public class AreaWeaponController : MonoBehaviour
{
    public AreaWeapon weapon;
    private Vector3 targetSize;
    private float timer;
    public List<EnemyController> enemiesinRange;
    private float counter;

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

        // daño periodico
        counter -= Time.deltaTime;
        if (counter <= 0)
        {
            counter = weapon.speed;
            foreach (EnemyController enemy in enemiesinRange)
            {
                enemy.TakeDamage(weapon.damage);
            }
        }

    }

    // private void OnTriggerStay2D(Collider2D collider)
    // {
    //     if (collider.CompareTag("Enemy"))
    //     {
    //         EnemyController enemy = collider.GetComponent<EnemyController>();
    //         enemy.TakeDamage(weapon.damage);
    //     }
    // }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            enemiesinRange.Add(collider.GetComponent<EnemyController>());
        }

    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            enemiesinRange.Remove(collider.GetComponent<EnemyController>());
        }
    }

}
