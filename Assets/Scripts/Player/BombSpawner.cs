using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombSpawner : MonoBehaviour
{
    [SerializeField] private Transform _positionToSpawn;
    [SerializeField] private GameObject _bombPrefab;

    public void SpawnBomb()
    {
        Instantiate(_bombPrefab, _positionToSpawn.position, Quaternion.identity);
    }
}