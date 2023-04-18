using UnityEngine;
using Random = UnityEngine.Random;

public class Reposition : MonoBehaviour
{
    private Transform playerTransform;
    private Collider2D _coll;

    private void Awake()
    {
        _coll = GetComponent<Collider2D>();
    }

    private void Start()
    {
        // Playerのtransform取得
        playerTransform = GameManager.Instance.PlayerController.transform;
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        // Areaでない場合リターン
        if (!col.CompareTag(GameDefine.Tag.Area.ToString()))
            return;
        
        // 現在のタイルマップの位置
        Vector3 myPos = transform.position;
        
        switch (transform.tag)
        { 
            // タイルマップのタグがTileの場合
            case "Tile":
                // PlayerとタイルマップのX、Y軸距離差計算
                float diffX = playerTransform.position.x - myPos.x;
                float diffY = playerTransform.position.y - myPos.y;
                float dirX = diffX < 0 ? -1 : 1;
                float dirY = diffY < 0 ? -1 : 1;
                diffX = Mathf.Abs(diffX);
                diffY = Mathf.Abs(diffY);
                
                // X、 Y軸距離差の中で大きいものを基準にタイルマップを移動
                // タイルマップ サイズ 40
                // diffXの符号に従ってタイルマップを右または左に移動
                if (diffX > diffY)
                    transform.Translate(Vector3.right * dirX * 40);
                // diffYの符号に従ってタイルマップを上または下に移動
                else if (diffX < diffY)
                    transform.Translate(Vector3.up * dirY * 40);
                break;
            case "Enemy":
                if (_coll.enabled)
                {
                    Vector3 dist = playerTransform.position - myPos;
                    Vector3 ran = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0);
                    transform.Translate(ran + dist * 2);
                }
                break;
        }
    }
}
