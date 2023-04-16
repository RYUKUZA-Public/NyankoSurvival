using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float Damage { get; private set; }
    private int _per;
    private Rigidbody2D _rigid;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }

    public void Init(float damage, int per, Vector3 dir)
    {
        Damage = damage;
        this._per = per;

        if (per >= 0)
            _rigid.velocity = dir * 15f;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Enemy") || _per == -100)
            return;

        _per--;

        if (_per < 0)
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
        if (!col.CompareTag("Area") || _per == -100)
            return;
        
        gameObject.SetActive(false);
    }
}
