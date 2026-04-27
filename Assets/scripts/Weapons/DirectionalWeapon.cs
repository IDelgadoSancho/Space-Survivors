using UnityEngine;

public class DirectionalWeapon : Weapon
{
    [SerializeField] private GameObject prefab;

    private float spawnCounter;

    void Update()
    {
        spawnCounter -= Time.deltaTime;
        if (spawnCounter <= 0)
        {
            if (!HasEnemyInRange()) return;

            spawnCounter = stats[weaponLevel].cooldown;

            StartCoroutine(FireBurst());
        }
    }

    private bool HasEnemyInRange()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float range = stats[weaponLevel].range;

        foreach (GameObject enemy in enemies)
        {
            if (Vector2.Distance(transform.position, enemy.transform.position) <= range)
                return true;
        }

        return false;
    }

    private System.Collections.IEnumerator FireBurst()
    {
        int amount = (int)stats[weaponLevel].amount;

        for (int i = 0; i < amount; i++)
        {
            GameObject obj = Instantiate(prefab, transform.position, transform.rotation);
            obj.GetComponent<DirectionalWeaponController>().weapon = this;

            yield return new WaitForSeconds(0.2f);
        }
    }
}
