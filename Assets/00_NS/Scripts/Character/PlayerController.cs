using UnityEngine;

public class PlayerController : NewMonoBehaviour
{
    /// <summary>
    /// ジョイスティック
    /// </summary>
    [SerializeField]
    private VariableJoystick joy;
    /// <summary>
    /// Playerごとにアニメが違うので
    /// </summary>
    [SerializeField]
    private RuntimeAnimatorController[] animeCon;
    /// <summary>
    /// Player移動速度
    /// </summary>
    [SerializeField]
    private float speed;
    public float Speed { get { return speed; } set { speed = value; } }
    /// <summary>
    /// Player入力ベクトル
    /// </summary>
    private Vector2 InputVec { get; set; }
    /// <summary>
    /// 近いEnemyを追跡
    /// </summary>
    public EnemyScan Scan { get; private set; }
    /// <summary>
    /// プレーヤー装着中の武器(演出用)
    /// </summary>
    public WeaponFlipper[] Hands { get; private set; }
    
    /// <summary>
    /// Player Rigidbody 2D コンポーネント
    /// </summary>
    private Rigidbody2D _rigid;
    /// <summary>
    /// PlayerのSprite
    /// </summary>
    private SpriteRenderer _sprite;
    /// <summary>
    /// PlayerのAnimator
    /// </summary>
    private Animator _animator;

    /// <summary>
    /// 初期化
    /// </summary>
    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _sprite = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        Scan = GetComponent<EnemyScan>();
        Hands = GetComponentsInChildren<WeaponFlipper>(true);
    }

    /// <summary>
    /// UpdateManager
    /// Poolを利用するため、OnEnableでデータ初期化
    /// </summary>
    protected override void OnEnable()
    {
        base.OnEnable();
        
        // キャラクターの固有能力によって基準が変更される
        speed *= CharacterSpecialAbility.Speed;
        _animator.runtimeAnimatorController = animeCon[GameManager.Instance.PlayerId];
    }
    
    /// <summary>
    /// Update Manager
    /// </summary>
    public override void NewFixedUpdate()
    {
        // Play中でなければ無視
        if (!GameManager.Instance.IsLive)
            return;
        
        // ジョイスティック操作した値を取得
        InputVec = new Vector2(joy.Horizontal, joy.Vertical);
        
        // 入力ベクトルと速度を利用して、Playerの次の位置ベクトル計算
        Vector2 nextVec = InputVec * (speed * Time.fixedDeltaTime);
        // Rigidbody2Dを利用してプレイヤー移動
        _rigid.MovePosition(_rigid.position + nextVec);
    }
    
    /// <summary>
    /// Update Manager
    /// </summary>
    public override void NewLateUpdate()
    {
        // Play中でなければ無視
        if (!GameManager.Instance.IsLive)
            return;
        
        // Speedを利用して、Player Idle <-> Move アニメ変更
        _animator.SetFloat( GameDefine.SpeedKey, InputVec.magnitude);
        
        // 入力ベクトルのx値が、0でなければ、スプライトの方向を変更
        if (InputVec.x != 0)
            _sprite.flipX = InputVec.x > 0;
    }
    
    /// <summary>
    /// Playerが攻撃を受けた時
    /// </summary>
    private void OnCollisionStay2D(Collision2D collision)
    {
        // Play中でなければ無視
        if (!GameManager.Instance.IsLive)
            return;
        
        // 近接衝突に対するStay
        // 時間の経過とともに体力を減少
        GameManager.Instance.Hp -= Time.deltaTime * 10;

        // プレイヤー死亡
        if (GameManager.Instance.Hp < 0)
        {
            for (int i = 1; i < transform.childCount; i++)
                transform.GetChild(i).gameObject.SetActive(false);
            
            _animator.SetTrigger(GameDefine.DeadKey);
            GameManager.Instance.GameOver();
        }
    }
    
    /// <summary>
    /// Time.timeScaleが0の場合、
    /// FixedUpdate関数が実行されないため、ここで停止する
    /// </summary>
    public void InitTimeStopPlayer()
    {
        if (!GameManager.Instance.IsLive)
        {
            InputVec = Vector2.zero;
            _rigid.velocity = Vector2.zero;
        }
    }
}
