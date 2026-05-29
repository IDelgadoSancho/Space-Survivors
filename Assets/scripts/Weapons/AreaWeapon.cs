using Unity.VisualScripting;
using UnityEngine;

public class AreaWeapon : Weapon
{
   [SerializeField] private GameObject prefab;
   private float spawnCounter;
   
   void Update()
    {    
        if (PlayerController.Instance.isDead) return;
        
        spawnCounter -= Time.deltaTime;
        if (spawnCounter <= 0)
        {
            spawnCounter = stats[weaponLevel].cooldown;
            Instantiate(prefab, transform.position, transform.rotation, transform);
        }


    }
}
