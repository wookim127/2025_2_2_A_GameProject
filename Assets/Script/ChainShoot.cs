using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainShoot : MonoBehaviour
{
    [Header("Chain Settings")]
    [SerializeField] float refreshRate = 0.05f;
    [SerializeField][Range(1, 10)] int maximumEnemiesInChain = 3;
    [SerializeField] float delayBetweenEachChain = 0.3f;

    [Header("References")]
    [SerializeField] Transform playerFirePoint;
    [SerializeField] EnemyDetector playerEnemyDetector;
    [SerializeField] GameObject lineRendererPrefab;

    bool shooting = false;
    bool shot = false;
    int chainCount = 1;

    List<GameObject> spawnedLineRenderers = new List<GameObject>();
    List<GameObject> enemiesInChain = new List<GameObject>();


    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            if (playerEnemyDetector.GetEnemiesInRange().Count > 0)
            {
                if (!shooting)
                    StartShooting();
            }
            else
            {
                StopShooting();
            }
        }

        if (Input.GetButtonUp("Fire1"))
        {
            StopShooting();
        }
    }


    void StartShooting()
    {
        shooting = true;

        if (!shot)
        {
            shot = true;
            GameObject firstEnemy = playerEnemyDetector.GetClosestEnemy();

            if (firstEnemy == null)
            {
                StopShooting();
                return;
            }

            chainCount = 1;
            enemiesInChain.Clear();
            enemiesInChain.Add(firstEnemy);

            CreateLineRenderer(playerFirePoint, firstEnemy.transform, true);

            if (maximumEnemiesInChain > 1)
                StartCoroutine(ChainReaction(firstEnemy));
        }
    }


    IEnumerator ChainReaction(GameObject currentEnemy)
    {
        yield return new WaitForSeconds(delayBetweenEachChain);

        if (!shooting) yield break;

        if (chainCount >= maximumEnemiesInChain) yield break;

        EnemyDetector detector = currentEnemy.GetComponent<EnemyDetector>();
        if (detector == null) yield break;

        GameObject nextEnemy = detector.GetClosestEnemy();

        if (nextEnemy == null) yield break;
        if (enemiesInChain.Contains(nextEnemy)) yield break;

        chainCount++;
        enemiesInChain.Add(nextEnemy);

        CreateLineRenderer(currentEnemy.transform, nextEnemy.transform);

        StartCoroutine(ChainReaction(nextEnemy));
    }


    void StopShooting()
    {
        shooting = false;
        shot = false;

        foreach (var lr in spawnedLineRenderers)
            Destroy(lr);

        spawnedLineRenderers.Clear();
        enemiesInChain.Clear();
    }


    // ─────────────────────────────────────────────────────────────────────────
    // LINE RENDERER 생성 및 실시간 갱신
    // ─────────────────────────────────────────────────────────────────────────
    void CreateLineRenderer(Transform start, Transform end, bool updateContinuously = false)
    {
        GameObject lrObj = Instantiate(lineRendererPrefab);
        LineRenderer lr = lrObj.GetComponent<LineRenderer>();

        lr.positionCount = 2;
        lr.SetPosition(0, start.position);
        lr.SetPosition(1, end.position);

        spawnedLineRenderers.Add(lrObj);

        if (updateContinuously)
        {
            StartCoroutine(UpdateLineRenderer(lr, start, end));
        }
    }


    IEnumerator UpdateLineRenderer(LineRenderer lr, Transform start, Transform end)
    {
        while (shooting && lr != null)
        {
            lr.SetPosition(0, start.position);
            lr.SetPosition(1, end.position);

            yield return new WaitForSeconds(refreshRate);
        }
    }
}
