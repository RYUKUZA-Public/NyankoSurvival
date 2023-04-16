using UnityEngine;

public class WeaponFlipper : MonoBehaviour
{
    public SpriteRenderer sprite;
    private SpriteRenderer _player;
    
    private Vector3 _localPos;
    private Quaternion _localRot;

    private void OnEnable()
    {
        _player = transform.parent.GetComponent<SpriteRenderer>();
        _localPos = transform.localPosition;
        _localRot = transform.localRotation;
        UpdateHand();
    }
    
    private void LateUpdate()
    {
        UpdateHand();
    }
    
    private void UpdateHand()
    {
        bool isReverse = _player.flipX;
        if (sprite.flipX != isReverse) {
            sprite.flipX = isReverse;
            transform.localPosition = isReverse ? new Vector3(-_localPos.x, _localPos.y, _localPos.z) : _localPos;
            transform.localRotation = isReverse ? Quaternion.Euler(_localRot.eulerAngles.x, _localRot.eulerAngles.y, -_localRot.eulerAngles.z) : _localRot;
        }
    }
}
