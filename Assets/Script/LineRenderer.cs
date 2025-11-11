using System.Collections.Generic;
using UnityEngine;

public class LineRendererController : MonoBehaviour
{
    [SerializeField] private List<UnityEngine.LineRenderer> lineRenderers = new List<UnityEngine.LineRenderer>();

    // startPos와 endPos 사이를 라인으로 연결
    public void SetPosition(Transform startPos, Transform endPos)
    {
        if (lineRenderers.Count == 0)
        {
            Debug.LogWarning("라인 렌더러가 설정되지 않았습니다.");
            return;
        }

        foreach (var lr in lineRenderers)
        {
            if (lr.positionCount < 2)
            {
                Debug.LogWarning($"{lr.name} 오브젝트의 LineRenderer에는 최소 2개의 점이 필요합니다.");
                continue;
            }

            lr.SetPosition(0, startPos.position);
            lr.SetPosition(1, endPos.position);
        }
    }
}
