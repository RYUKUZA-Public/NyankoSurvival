using System;
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
        playerTransform = GameManager.Instance.Player.transform;
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        // Areaでない場合リターン
        if (!col.CompareTag("Area"))
            return;
        
        // 現在のタイルマップの位置
        Vector3 myPos = transform.position;
        // Playerの入力方向
        Vector3 playerDir = GameManager.Instance.Player.InputVec;
        
        // PlayerとタイルマップのX、Y軸距離差計算
        float diffX = playerTransform.position.x - myPos.x;
        float diffY = playerTransform.position.y - myPos.y;
        
        switch (transform.tag)
        {
            // タイルマップのタグがTileの場合
            case "Tile":
                // X、 Y軸距離差の中で大きいものを基準にタイルマップを移動
                if (Mathf.Abs(diffX) > Mathf.Abs(diffY))
                {
                    // タイルマップ サイズ 40
                    // diffXの符号に従ってタイルマップを右または左に移動
                    transform.Translate(Vector3.right * Mathf.Sign(diffX) * 40);
                    
                }
                else if (Mathf.Abs(diffX) < Mathf.Abs(diffY))
                {
                    // diffYの符号に従ってタイルマップを上または下に移動
                    transform.Translate(Vector3.up * Mathf.Sign(diffY) * 40);
                }
                break;
            case "Enemy":
                // TODO. Enemyが生きている時だけ
                if (_coll.enabled)
                {
                    transform.Translate(playerDir * 20 + new Vector3
                        (Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0));
                }
                break;
        }
    }
}
