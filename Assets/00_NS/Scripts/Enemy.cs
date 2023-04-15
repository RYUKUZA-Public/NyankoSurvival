using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
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
    private SpriteRenderer _sprite;
    private Animator _animator;
    private WaitForFixedUpdate _wait;
    
    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _sprite = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _wait = new WaitForFixedUpdate();
    }

    private void OnEnable()
    {
        // 活性時に、Target(Player)登録
        target = GameManager.Instance.Player.GetComponent<Rigidbody2D>();
        _isLive = true;
        hp = maxHp;
    }

    public void Init(SpawnData data)
    {
        speed = data.Speed;
        maxHp = data.Hp;
        hp = data.Hp;
    }

    private void FixedUpdate()
    {
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
        if (!_isLive)
            return;
        
        // Playerが、Enemyより左側にあるときEnemy方向転換
        _sprite.flipX = target.position.x < _rigid.position.x;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Bullet"))
            return;

        hp -= col.GetComponent<Bullet>().Damage;
        StartCoroutine(KnockBack());

        // Hit
        if (hp > 0)
        {
            _animator.SetTrigger("Hit");
        }
        else
        {
            // Dead
            Dead();
        }
        
    }

    private IEnumerator KnockBack()
    {
        yield return _wait;
        Vector3 playerPos = GameManager.Instance.Player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        _rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
        
        Debug.Log("1111111111111111111");
    }

    private void Dead()
    {
        _animator.SetBool("Dead", true);
        
        asd(() =>
        {
            gameObject.SetActive(false);
        });
    }

    private Action call;

    private void asd(Action call2)
    {
        call = call2;
    }

    public void DeadAniCall()
    {
        call?.Invoke();
    }
}
