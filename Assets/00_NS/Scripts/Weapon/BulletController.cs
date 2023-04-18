using UnityEngine;

public class BulletController : MonoBehaviour
{
    [Header("EnemyCleaner")]
    [SerializeField]
    private bool isEnemyCleaner;
    private bool _cleanerDamage;
    
    public float Damage { get; private set; }
    private int _penetration;
    private Rigidbody2D _rigid;
    
    /// <summary>
    /// 初期化
    /// </summary>
    private void Awake()
    {
        if (isEnemyCleaner)
        {
            Damage = 1000000f;
            _penetration = -100;
        }
        
        _rigid = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// データ初期化
    /// </summary>
    public void Init(float damage, int penetration, Vector3 direction)
    {
        Damage = damage;
        this._penetration = penetration;

        if (penetration >= 0)
            _rigid.velocity = direction * 15f;
    }

    /// <summary>
    /// 発射した弾丸の貫通機能
    /// </summary>
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag(GameDefine.Tag.Enemy.ToString()) || _penetration == -100)
            return;
        
        // 貫通可能回数を1減少
        _penetration--;
        
        // 貫通可能、回数をすべて使用した場合
        if (_penetration < 0)
        {
            _rigid.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 弾丸が範囲外に出る場合、非活性
    /// </summary>
    private void OnTriggerExit2D(Collider2D col)
    {
        if (!col.CompareTag(GameDefine.Tag.Area.ToString()) || _penetration == -100)
            return;
        
        gameObject.SetActive(false);
    }
}
