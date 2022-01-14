using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bomb : MonoBehaviour
{
    public int explodeSize = 3;
    public GameObject explosionPrefab;

    private LevelStatsComponent _levelStats;
    private Canvas _canvas;
    private Vector2 _originPos;


    private void Awake()
    {
        _canvas = FindObjectOfType(typeof(Canvas)) as Canvas;
        if (_canvas != null) _levelStats = _canvas.GetComponent<LevelStatsComponent>();
    }

    void Start()
    {
        Invoke(nameof(Explode), 1.5f);

        _originPos = transform.position;
    }


    void Explode()
    {
        Instantiate(explosionPrefab, (Vector2) transform.position, Quaternion.identity);


        StartCoroutine(CreateExplosions(Vector2.up));
        StartCoroutine(CreateExplosions(Vector2.right));
        StartCoroutine(CreateExplosions(Vector2.down));
        StartCoroutine(CreateExplosions(Vector2.left));


        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<BoxCollider2D>().isTrigger = false;
        Destroy(gameObject, .3f);
    }

    private IEnumerator CreateExplosions(Vector2 direction)
    {
        List<Vector2> instantiateList = new List<Vector2>();

        for (int i = 1; i <= explodeSize; i++)
        {
            RaycastHit2D hit;

            if (direction.x > direction.y)
            {
                hit = Physics2D.CapsuleCast(_originPos, new Vector2(3, 1), CapsuleDirection2D.Horizontal, 0f,
                    direction);
            }
            else
            {
                hit = Physics2D.CapsuleCast(_originPos, new Vector2(3, 1), CapsuleDirection2D.Vertical, 0f, direction);
            }


            if (!hit.collider)
            {
                instantiateList.Add((Vector2) transform.position + i * direction);
            }
            else
            {
                if (hit.collider.CompareTag("Player") || hit.collider.CompareTag("Enemy"))
                {
                    if (hit.collider.gameObject.GetComponentInChildren<HealthBarComponent>() != null)
                    {
                        hit.collider.gameObject.GetComponentInChildren<HealthBarComponent>().Damage(100);
                        GameOverComponent.Loss();
                        continue;
                    }

                    if (hit.collider.CompareTag("Enemy"))
                    {
                        _levelStats.DeadEnemyCount += 1;
                        hit.collider.gameObject.SetActive(false);
                    }


                    instantiateList.Add((Vector2) transform.position + (i * direction));
                }

                if (hit.collider.CompareTag("Block"))
                {
                    hit.collider.gameObject.SetActive(false);
                }


                break;
            }
        }

        foreach (Vector2 position in instantiateList)
        {
            Instantiate(explosionPrefab, position,
                explosionPrefab.transform.rotation);

            yield return new WaitForSeconds(.05f);
        }
    }
}