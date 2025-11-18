using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyDetector : MonoBehaviour
{
    public float detectRange = 10f;
    public LayerMask enemyMask;

    public List<GameObject> GetEnemiesInRange()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectRange, enemyMask);
        List<GameObject> list = new List<GameObject>();

        foreach (var h in hits)
            list.Add(h.gameObject);

        return list;
    }

    public GameObject GetClosestEnemy()
    {
        var list = GetEnemiesInRange();
        if (list.Count == 0) return null;

        return list.OrderBy(e => Vector3.Distance(transform.position, e.transform.position)).FirstOrDefault();
    }
}
