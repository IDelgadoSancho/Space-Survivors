using UnityEngine;

public class SpinWeaponProjectile : MonoBehaviour
{
    private SpinWeapon weapon;

    void Start()
    {
        weapon = GameObject.Find("Plasma Ball").GetComponent<SpinWeapon>();
    }

    private void OnTriggerEnter2D(Collider2D collider){
        if (collider.gameObject.CompareTag("Enemy")){
            EnemyController enemy = collider.gameObject.GetComponent<EnemyController>();
            enemy.TakeDamage(weapon.stats[weapon.weaponLevel].damage);
        }
    }
}
