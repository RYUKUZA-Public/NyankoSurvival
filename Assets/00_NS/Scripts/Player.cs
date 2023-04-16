using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    /// <summary>
    /// Player入力ベクトル
    /// </summary>
    [SerializeField]
    private Vector2 inputVec;
    public Vector2 InputVec => inputVec;
    public EnemyScan Scan { get; set; }

    public Hand[] hands;

    public RuntimeAnimatorController[] animeCon;
    
    
    /// <summary>
    /// Player移動速度
    /// </summary>
    [SerializeField]
    private float speed;
    public float Speed { get { return speed; } set { speed = value; } }
    
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

    private void Awake()
    {
        // Rigidbody2D 初期化
        _rigid = GetComponent<Rigidbody2D>();
        _sprite = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        Scan = GetComponent<EnemyScan>();
        hands = GetComponentsInChildren<Hand>(true);
    }

    private void OnEnable()
    {
        speed *= Character.Speed;
        _animator.runtimeAnimatorController = animeCon[GameManager.Instance.PlayerId];
    }

    public void InitTimeStopPlayer()
    {
        // Time.timeScaleが0の場合、FixedUpdate関数が実行されないため、ここで停止する
        if (!GameManager.Instance.IsLive)
        {
            inputVec = Vector2.zero;
            _rigid.velocity = Vector2.zero;
        }
    }

    private void FixedUpdate()
    {
        if (!GameManager.Instance.IsLive)
            return;
        
        // 入力ベクトルと速度を利用して、Playerの次の位置ベクトル計算
        Vector2 nextVec = inputVec * (speed * Time.fixedDeltaTime);
        // Rigidbody2Dを利用してプレイヤー移動
        _rigid.MovePosition(_rigid.position + nextVec);
    }
    
    private void LateUpdate()
    {
        if (!GameManager.Instance.IsLive)
            return;
        
        // Speedを利用して、Player Idle <-> Move アニメ変更
        _animator.SetFloat("Speed", inputVec.magnitude);
        
        // 入力ベクトルのx値が、0でなければ、スプライトの方向を変更
        if (inputVec.x != 0)
            _sprite.flipX = inputVec.x > 0;
    }

    /// <summary>
    /// InputSystemのイベント関数
    /// </summary>
    private void OnMove(InputValue value)
    {
        if (!GameManager.Instance.IsLive)
            return;
        
        // 入力値をVector2形式に、変換して入力ベクトルに保存
        inputVec = value.Get<Vector2>();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!GameManager.Instance.IsLive)
            return;

        GameManager.Instance.Hp -= Time.deltaTime * 10;

        if (GameManager.Instance.Hp < 0)
        {
            // Dead
            for (int i = 1; i < transform.childCount; i++)
                transform.GetChild(i).gameObject.SetActive(false);
            
            _animator.SetTrigger("Dead");
            GameManager.Instance.GameOver();
        }

    }
}
