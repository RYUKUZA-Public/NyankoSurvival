using System.Collections;
using UnityEngine;

public class EnemyController : NewMonoBehaviour
{
    /// <summary>
    /// Enemy 移動速度
    /// </summary>
    private float Speed { get; set; }
    /// <summary>
    /// Enemy 体力
    /// </summary>
    private float Hp { get; set; }
    /// <summary>
    /// Enemy 最大体力
    /// </summary>
    private float MaxHp { get; set; }
    /// <summary>
    /// 追跡対象 (Player)
    /// </summary>
    private Rigidbody2D Target { get; set; }

    /// <summary>
    /// Enemy 生存の有無
    /// </summary>
    private bool _isLive;

    private Rigidbody2D _rigid;
    private Collider2D _coll;
    private SpriteRenderer _sprite;
    private Animator _animator;
    private WaitForFixedUpdate _wait;
    
    // private readonly string _hit = GameDefine.EnemyAnimationType.Hit.ToString();
    // private readonly string _dead = GameDefine.EnemyAnimationType.Dead.ToString();
    
    /// <summary>
    /// 初期化
    /// </summary>
    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _coll = GetComponent<Collider2D>();
        _sprite = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _wait = new WaitForFixedUpdate();
    }
    
    /// <summary>
    /// データ初期化
    /// </summary>
    public void Init(EnemySpawnData data)
    {
        Speed = data.Speed;
        MaxHp = data.Hp;
        Hp = data.Hp;
    }
    
    /// <summary>
    /// UpdateManager
    /// Poolを利用するため、OnEnableでデータ初期化
    /// </summary>
    protected override void OnEnable()
    {
        base.OnEnable();
        
        // 活性時に、Target(Player)登録
        Target = GameManager.Instance.PlayerController.GetComponent<Rigidbody2D>();
        Hp = MaxHp;
        _isLive = true;
        _coll.enabled = true;
        _rigid.simulated = true;
        _sprite.sortingOrder = 2;
        _animator.SetBool(GameDefine.DeadKey, false);
    }
    
    /// <summary>
    /// Update Manager
    /// </summary>
    public override void NewFixedUpdate()
    {
        // Play中でなければ無視
        if (!GameManager.Instance.IsLive)
            return;
        
        if (!_isLive || _animator.GetCurrentAnimatorStateInfo(0).IsName(GameDefine.HitKey))
            return;

        // PlayerとEnemyの間の方向ベクトル計算
        Vector2 dirVec = Target.position - _rigid.position;
        // 移動するベクトル計算
        Vector2 nextVec = dirVec.normalized * Speed * Time.fixedDeltaTime;
        // Enemy移動
        _rigid.MovePosition(_rigid.position + nextVec);
        // 速度初期化
        _rigid.velocity = Vector2.zero;
    }

    /// <summary>
    /// Update Manager
    /// </summary>
    public override void NewLateUpdate()
    {
        // Play中でなければ無視
        if (!GameManager.Instance.IsLive)
            return;
        // Enemy死亡時無視
        if (!_isLive)
            return;
        
        // Playerが、Enemyより左側にあるときEnemy方向転換
        _sprite.flipX = Target.position.x < _rigid.position.x;
    }
    
    /// <summary>
    /// Playerに攻撃された時
    /// </summary>
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Bullet") || !_isLive)
            return;
        
        // 体力減少
        Hp -= col.GetComponent<BulletController>().Damage;
        // ノックバックコルチン開始
        StartCoroutine(KnockBack());
        
        // 生きている
        if (Hp > 0)
        {
            // Hitアニメ再生
            _animator.SetTrigger(GameDefine.HitKey);
            AudioManager.Instance.PlaySfx(GameDefine.Sfx.Hit);
        }
        // 死亡
        else
        {
            _isLive = false;
            _coll.enabled = false;
            _rigid.simulated = false;
            _sprite.sortingOrder = 1;
            _animator.SetBool(GameDefine.DeadKey, true);
            GameManager.Instance.Kill++;
            GameManager.Instance.GetExp();
            
            // これを抜くと、最後の全体攻撃時に一斉に再生される。気をつけろ
            if (GameManager.Instance.IsLive)
                AudioManager.Instance.PlaySfx(GameDefine.Sfx.Dead);
        }
    }

    /// <summary>
    /// ノックバック
    /// </summary>
    private IEnumerator KnockBack()
    {
        yield return _wait;
        Vector3 playerPos = GameManager.Instance.PlayerController.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        _rigid.AddForce(dirVec.normalized * GameDefine.EnemyKnockBackPower, ForceMode2D.Impulse);
    }
    
    /// <summary>
    /// Deadアニメの最後に、コールバックに使用
    /// Poolを使用するため削除しない
    /// </summary>
    private void Dead() => gameObject.SetActive(false);
}
