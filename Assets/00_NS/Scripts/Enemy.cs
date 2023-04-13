using UnityEngine;

public class Enemy : MonoBehaviour
{
    /// <summary>
    /// Enemy 移動速度
    /// </summary>
    [SerializeField]
    private float speed;
    /// <summary>
    /// 追跡対象 (Player)
    /// </summary>
    [SerializeField]
    private Rigidbody2D target;

    /// <summary>
    /// TODO. TEST
    /// Enemy 生存の有無
    /// </summary>
    private bool _isLive = true;

    private Rigidbody2D _rigid;
    private SpriteRenderer _sprite;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _sprite = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if (!_isLive)
            return;

        // PlayerとEnemyの間の方向ベクトル計算
        Vector2 dirVec = target.position - _rigid.position;
        // 移動するベクトル計算
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        // Enemy移動
        _rigid.MovePosition(_rigid.position + nextVec);
        // 速度初期化
        _rigid.velocity = Vector2.zero;
    }

    private void LateUpdate()
    {
        if (!_isLive)
            return;
        
        // Playerが、Enemyより左側にあるときEnemy方向転換
        _sprite.flipX = target.position.x < _rigid.position.x;
    }
}
