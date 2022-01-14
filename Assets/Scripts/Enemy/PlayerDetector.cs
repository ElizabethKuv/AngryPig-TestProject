using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    [SerializeField] private Transform _detectorOriginPos;

    public Vector2 detectorSize = Vector2.one;
    public Vector2 detectorOffsetFromOrigin = Vector2.zero;

    public float detectionDelay = 0.3f;
    public LayerMask detectorLayerMask;

    [Header("Gizmos")] public Color gizmosIdleColor = Color.green;
    public Color gizmosDetectedColor = Color.red;
    public bool isShowingGizmos = true;

    private GameObject _target;

    public GameObject Target
    {
        get => _target;
        private set
        {
            _target = value;
            EnemyAI.PlayerInAreaToDamage = (_target != null);
        }
    }

    private void Start()
    {
        StartCoroutine(DetectionCoroutine());
    }

    private IEnumerator DetectionCoroutine()
    {
        yield return new WaitForSeconds(detectionDelay);
        PerformDetection();
        StartCoroutine(DetectionCoroutine());
    }

    private void PerformDetection()
    {
        Collider2D collider = Physics2D.OverlapBox((Vector2) _detectorOriginPos.position + detectorOffsetFromOrigin,
            detectorSize, 0, detectorLayerMask);

        if (collider != null)
        {
            Target = collider.gameObject;
        }
        else
        {
            Target = null;
        }
    }

    private void OnDrawGizmos()
    {
        if (isShowingGizmos && _detectorOriginPos != null)
        {
            Gizmos.color = gizmosIdleColor;
            if (EnemyAI.PlayerInAreaToDamage)
            {
                Gizmos.color = gizmosDetectedColor;
                Gizmos.DrawCube((Vector2) _detectorOriginPos.position + detectorOffsetFromOrigin,
                    detectorSize);
            }
        }
    }
}