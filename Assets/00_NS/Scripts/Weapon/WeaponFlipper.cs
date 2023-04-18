using UnityEngine;

public class WeaponFlipper : NewMonoBehaviour
{
    public SpriteRenderer sprite;
    private SpriteRenderer _player;
    
    private Vector3 _localPos;
    private Quaternion _localRot;

    /// <summary>
    /// Update Manager
    /// 初期化
    /// </summary>
    protected override void OnEnable()
    {
        base.OnEnable();
        _player = transform.parent.GetComponent<SpriteRenderer>();
        _localPos = transform.localPosition;
        _localRot = transform.localRotation;
        
        //初期化時点で、武器の位置と回転を更新
        UpdateHand();
    }
    
    /// <summary>
    /// Update Manager
    /// 武器の位置と回転を更新
    /// </summary>
    public override void NewLateUpdate()
    {
        UpdateHand();
    }
    
    /// <summary>
    /// 武器の位置と回転を更新
    /// </summary>
    private void UpdateHand()
    {
        // Playerが反転した状態であることを確認
        bool isReverse = _player.flipX;
        
        // 武器が、Playerの反転状態と異なる場合
        if (sprite.flipX != isReverse) {
            sprite.flipX = isReverse;
            
            transform.localPosition = isReverse ? new Vector3(
                -_localPos.x, _localPos.y, _localPos.z) : _localPos;
            
            transform.localRotation = isReverse ? Quaternion.Euler(
                _localRot.eulerAngles.x, _localRot.eulerAngles.y, -_localRot.eulerAngles.z) : _localRot;
        }
    }
}
