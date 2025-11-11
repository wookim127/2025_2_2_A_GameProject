using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] private float detectionRadius = 10f;
    [SerializeField] private LayerMask enemyLayer;

    // 가장 가까운 적 찾기
    public GameObject GetClosestEnemy()
    {
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, detectionRadius, enemyLayer);

        if (enemiesInRange.Length == 0)
            return null;

        GameObject bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (Collider enemyCollider in enemiesInRange)
        {
            if (enemyCollider.gameObject == gameObject)
                continue; // 자기 자신은 무시

            Vector3 directionToTarget = enemyCollider.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;

            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = enemyCollider.gameObject;
            }
        }

        return bestTarget;
    }

    // 탐지 반경 내의 모든 적 리스트 반환
    public List<GameObject> GetEnemiesInRange()
    {
        List<GameObject> enemiesList = new List<GameObject>();
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, detectionRadius, enemyLayer);

        foreach (Collider enemyCollider in enemiesInRange)
        {
            if (enemyCollider.gameObject != gameObject) // 자기 자신 제외
                enemiesList.Add(enemyCollider.gameObject);
        }

        return enemiesList;
    }

    // 에디터에서 탐지 반경 시각화
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
