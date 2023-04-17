using UnityEngine;

public class NewMonoBehaviour : MonoBehaviour
{
    /// <summary>
    /// 実行時、UpdateManagerに追加
    /// </summary>
    [SerializeField] private bool addOnEnable = true;
    /// <summary>
    /// 終了時、UpdateManagerから削除
    /// </summary>
    [SerializeField] private bool removeOnDisable = true;

    protected virtual void OnEnable()
    {
        if (!addOnEnable)
            return;
        
        UpdateManager.AddItem(this);
    }

    protected virtual void OnDisable()
    {
        if (!removeOnDisable)
            return;
        
        UpdateManager.RemoveSpecificItem(this);
    }

    public virtual void NewUpdate() { }

    public virtual void NewFixedUpdate() { }

    public virtual void NewLateUpdate() { }
}