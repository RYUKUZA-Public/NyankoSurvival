using UnityEngine;

public class Hand : MonoBehaviour
{
    public SpriteRenderer sprite;
    private SpriteRenderer _player;
    
    private Vector3 _localPos;
    private Quaternion _localRot;
    private bool _previousIsReverse;

    private void Awake()
    {
        _player = transform.parent.GetComponent<SpriteRenderer>();
        _localPos = transform.localPosition;
        _localRot = transform.localRotation;
        _previousIsReverse = _player.flipX;
    }

    private void LateUpdate()
    {
        bool isReverse = _player.flipX;
        if (isReverse != _previousIsReverse) {
            transform.localPosition = isReverse ? new Vector3(-_localPos.x, _localPos.y, _localPos.z) : _localPos;
            transform.localRotation = isReverse ? Quaternion.Euler(_localRot.eulerAngles.x, _localRot.eulerAngles.y, -_localRot.eulerAngles.z) : _localRot;
            sprite.flipX = isReverse;
            _previousIsReverse = isReverse;
        }
    }
}
