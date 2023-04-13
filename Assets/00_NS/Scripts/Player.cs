using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    /// <summary>
    /// Player入力ベクトル
    /// </summary>
    [SerializeField]
    private Vector2 inputVec;
    /// <summary>
    /// Player移動速度
    /// </summary>
    [SerializeField]
    private float speed;
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
    }

    private void FixedUpdate()
    {
        // 入力ベクトルと速度を利用して、Playerの次の位置ベクトル計算
        Vector2 nextVec = inputVec * (speed * Time.fixedDeltaTime);
        // Rigidbody2Dを利用してプレイヤー移動
        _rigid.MovePosition(_rigid.position + nextVec);
    }
    
    private void LateUpdate()
    {
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
        // 入力値をVector2形式に、変換して入力ベクトルに保存
        inputVec = value.Get<Vector2>();
    }
}
