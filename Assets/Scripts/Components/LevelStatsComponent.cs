using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStatsComponent : MonoBehaviour
{
    [SerializeField] private int _enemyCountAtAll;
    [SerializeField] private HealthBarComponent _healthBarComponent;

    private int _deadenemyCount;

    void Awake()
    {
        DeadEnemyCount = _deadenemyCount;
    }

    public int DeadEnemyCount
    {
        get => _deadenemyCount;
        set { _deadenemyCount = value; }
    }

    private void Update()
    {
        if (_enemyCountAtAll == DeadEnemyCount)
        {
            GameOverComponent.Won();
        }

        if (_healthBarComponent.currentHealth <= 0)
        {
            GameOverComponent.Loss();
        }
    }
}