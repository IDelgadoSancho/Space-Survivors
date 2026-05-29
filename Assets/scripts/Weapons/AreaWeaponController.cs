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
        weapon = GameObject.Find("Shield").GetComponent<AreaWeapon>();
        // Destroy(gameObject, weapon.duration);
        targetSize = Vector3.one * weapon.stats[weapon.weaponLevel].range;
        transform.localScale = Vector3.zero;
        timer = weapon.stats[weapon.weaponLevel].duration;
        AudioController.Instance.PlaySound(AudioController.Instance.areaWeapon);
    }

    void Update()
    {
        if (PlayerController.Instance.isDead)
        {
            Destroy(gameObject, weapon.stats[weapon.weaponLevel].duration);
            return;
        }

        //grow
        transform.localScale = Vector3.MoveTowards(transform.localScale, targetSize, Time.deltaTime * 5);
        //shrink
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            targetSize = Vector3.zero;
            if (transform.localScale.x == 0f)
            {
                Destroy(gameObject, weapon.stats[weapon.weaponLevel].duration);
            }
        }

        // daño periodico
        counter -= Time.deltaTime;
        if (counter <= 0)
        {
            counter = weapon.stats[weapon.weaponLevel].speed;
            foreach (EnemyController enemy in enemiesinRange)
            {
                enemy.TakeDamage(weapon.stats[weapon.weaponLevel].damage);
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
