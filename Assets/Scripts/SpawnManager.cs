﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject[] _powerups;
    private bool _stopSpawning = false;

    public void StartSpawning() {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    IEnumerator SpawnEnemyRoutine() {
        yield return new WaitForSeconds(1.0f);

        while (!_stopSpawning) {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(3.0f);
        }
    }

    IEnumerator SpawnPowerupRoutine() {
        yield return new WaitForSeconds(1.0f);
        
        while (!_stopSpawning) {
            int powerupID = Random.Range(0, 3);
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            Instantiate(_powerups[powerupID], posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(10f, 20f));
        }
    }

    public void OnPlayerDeath() {
        _stopSpawning = true;
    }
}
