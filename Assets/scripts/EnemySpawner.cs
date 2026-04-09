using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public GameObject enemyPrefab;
        public float spawnTimer;
        public float spawnInterval;
        public int enemiesPerWave;
        public int spawnedEnemyCount;
    }

    public enum SpawnType
    {
        Air,
        Ground
    }

    [Header("Wave Settings")]
    public List<Wave> waves;
    public int waveNumber;

    [Header("Spawn Type")]
    public SpawnType spawnType;

    [Header("Air Spawn")]
    public Transform minPos;
    public Transform maxPos;

    [Header("Ground Spawn")]
    public BoxCollider2D spawnArea;
    public float spawnMargin = 2f; // distancia mínima fuera del viewport

    // void OnDrawGizmos()
    // {
    //     if (spawnArea != null)
    //     {
    //         Gizmos.color = Color.green;
    //         Gizmos.DrawWireCube(spawnArea.bounds.center, spawnArea.bounds.size);
    //     }
    // }
    
    void Update()
    {
        if (PlayerController.Instance.gameObject.activeSelf == true)
        {
            Wave wave = waves[waveNumber];
            wave.spawnTimer += Time.deltaTime;

            if (wave.spawnTimer >= wave.spawnInterval)
            {
                wave.spawnTimer = 0;
                SpawnEnemy();
            }

            if (wave.spawnedEnemyCount >= wave.enemiesPerWave)
            {
                wave.spawnedEnemyCount = 0;

                if (wave.spawnInterval > 0.3f)
                    wave.spawnInterval *= 0.9f;

                waveNumber++;
            }

            if (waveNumber >= waves.Count)
                waveNumber = 0;
        }
    }

    private void SpawnEnemy()
    {
        Vector3 spawnPos = (spawnType == SpawnType.Air)
            ? RandomSpawnPointAir()
            : RandomSpawnPointGround();

        Instantiate(waves[waveNumber].enemyPrefab, spawnPos, Quaternion.identity);
        waves[waveNumber].spawnedEnemyCount++;
    }

    private Vector3 RandomSpawnPointAir()
    {
        Vector3 spawnPoint;

        if (Random.value > 0.5f)
        {
            spawnPoint.x = Random.Range(minPos.position.x, maxPos.position.x);
            spawnPoint.y = Random.value > 0.5f ? minPos.position.y : maxPos.position.y;
        }
        else
        {
            spawnPoint.y = Random.Range(minPos.position.y, maxPos.position.y);
            spawnPoint.x = Random.value > 0.5f ? minPos.position.x : maxPos.position.x;
        }

        spawnPoint.z = 0f;
        return spawnPoint;
    }

    private Vector3 RandomSpawnPointGround()
    {
        Bounds bounds = spawnArea.bounds;
        Camera cam = Camera.main;

        float camHeight = 2f * cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;

        float visibleLeft = cam.transform.position.x - camWidth / 2f - spawnMargin;
        float visibleRight = cam.transform.position.x + camWidth / 2f + spawnMargin;

        Vector3 spawnPoint;

        // Comprobar espacio
        bool canSpawnLeft = visibleLeft > bounds.min.x + 0.1f;
        bool canSpawnRight = visibleRight < bounds.max.x - 0.1f;

        if (canSpawnLeft && canSpawnRight)
        {
            // Ambos lados disponibles, elegimos aleatorio
            if (Random.value > 0.5f)
                spawnPoint.x = Random.Range(bounds.min.x, visibleLeft);
            else
                spawnPoint.x = Random.Range(visibleRight, bounds.max.x);
        }
        else if (canSpawnLeft)
        {
            spawnPoint.x = Random.Range(bounds.min.x, visibleLeft);
        }
        else if (canSpawnRight)
        {
            spawnPoint.x = Random.Range(visibleRight, bounds.max.x);
        }
        else
        {
            // No hay espacio fuera de la cámara, spawn en cualquier lugar dentro del collider
            spawnPoint.x = Random.Range(bounds.min.x, bounds.max.x);
        }

        // Y aleatoria dentro de los limites verticales del box collider
        spawnPoint.y = Random.Range(bounds.min.y + 0.1f, bounds.max.y - 0.1f);
        spawnPoint.z = 0f;

        return spawnPoint;
    }
}
