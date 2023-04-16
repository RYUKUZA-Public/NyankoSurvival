using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float damage;
    public float Damage => damage;
    [SerializeField]
    private int per;
    public int Per => per;

    private Rigidbody2D _rigid;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }

    public void Init(float damage, int per, Vector3 dir)
    {
        this.damage = damage;
        this.per = per;

        if (per >= 0)
            _rigid.velocity = dir * 15f;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Enemy") || per == -100)
            return;

        per--;

        if (per < 0)
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
        if (!col.CompareTag("Area") || per == -100)
            return;
        
        gameObject.SetActive(false);
    }
}
