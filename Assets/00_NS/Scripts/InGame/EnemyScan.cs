using UnityEngine;

public class EnemyScan : NewMonoBehaviour
{
    /// <summary>
    /// 探索範囲
    /// </summary>
    [SerializeField]
    private float scanRange;
    /// <summary>
    /// 探索するレイヤー
    /// </summary>
    [SerializeField] private LayerMask targetLayer;
    /// <summary>
    /// 一番近い敵
    /// </summary>
    public Transform NearestTarget { get; private set; }
    private Collider2D[] _targets;
    
    /// <summary>
    /// UpdateManager
    /// </summary>
    public override void NewFixedUpdate()
    {
        // 円形衝突チェックで敵を探知し、配列に保存
        _targets = Physics2D.OverlapCircleAll(transform.position, scanRange, targetLayer);
        // 最寄の対象探し
        NearestTarget = GetNearest();
    }

    /// <summary>
    /// 最寄の対象探し
    /// </summary>
    private Transform GetNearest()
    {
        Transform result = null;
        float diff = Mathf.Infinity;
        Vector3 currentPos = transform.position;

        foreach (Collider2D target in _targets)
        {
            // 現在地と敵地の間の距離を求める
            float currentDiff = Vector3.Distance(currentPos, target.transform.position);

            if (currentDiff < diff)
            {
                // 現在、最も近い敵より近い場合は結果を更新する
                diff = currentDiff;
                result = target.transform;
            }
        }

        return result;
    }
}
