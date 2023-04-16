using System;
using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    /// <summary>
    /// Enemy 移動速度
    /// </summary>
    [SerializeField]
    private float speed;
    [SerializeField]
    private float hp;
    [SerializeField]
    private float maxHp;
    /// <summary>
    /// 追跡対象 (Player)
    /// </summary>
    [SerializeField]
    private Rigidbody2D target;

    /// <summary>
    /// Enemy 生存の有無
    /// </summary>
    private bool _isLive;

    private Rigidbody2D _rigid;
    private Collider2D _coll;
    private SpriteRenderer _sprite;
    private Animator _animator;
    private WaitForFixedUpdate _wait;
    
    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _coll = GetComponent<Collider2D>();
        _sprite = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _wait = new WaitForFixedUpdate();
    }

    private void OnEnable()
    {
        // 活性時に、Target(Player)登録
        target = GameManager.Instance.PlayerController.GetComponent<Rigidbody2D>();
        _isLive = true;
        hp = maxHp;
        
        _isLive = true;
        _coll.enabled = true;
        _rigid.simulated = true;
        _sprite.sortingOrder = 2;
        _animator.SetBool("Dead", false);
    }

    public void Init(SpawnData data)
    {
        speed = data.Speed;
        maxHp = data.Hp;
        hp = data.Hp;
    }

    private void FixedUpdate()
    {
        if (!GameManager.Instance.IsLive)
            return;
        
        if (!_isLive || _animator.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
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
        if (!GameManager.Instance.IsLive)
            return;
        
        if (!_isLive)
            return;
        
        // Playerが、Enemyより左側にあるときEnemy方向転換
        _sprite.flipX = target.position.x < _rigid.position.x;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Bullet") || !_isLive)
            return;

        hp -= col.GetComponent<BulletController>().Damage;
        StartCoroutine(KnockBack());

        // Hit
        if (hp > 0)
        {
            _animator.SetTrigger("Hit");
            AudioManager.Instance.PlaySfx(AudioManager.Sfx.Hit);
        }
        else
        {
            _isLive = false;
            _coll.enabled = false;
            _rigid.simulated = false;
            _sprite.sortingOrder = 1;
            _animator.SetBool("Dead", true);
            GameManager.Instance.Kill++;
            GameManager.Instance.GetExp();
            if (GameManager.Instance.IsLive)
                AudioManager.Instance.PlaySfx(AudioManager.Sfx.Dead);
        }
    }

    private IEnumerator KnockBack()
    {
        yield return _wait;
        Vector3 playerPos = GameManager.Instance.PlayerController.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        _rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
    }

    private void Dead()
    {
        gameObject.SetActive(false);
    }
    
}
