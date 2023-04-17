using System;

public class UpdateManager : SingletonBehaviour<UpdateManager>
{
    protected override bool DoNotDestroyOnLoad { get { return false; } }

    /// <summary>
    /// Updateたちのイベント宣言
    /// </summary>
    private static event Action OnUpdateEvent;
    private static event Action OnFixedUpdateEvent;
    private static event Action OnLateUpdateEvent;
    
    /// <summary>
    /// NewMonoBehaviour タイプ取得
    /// </summary>
    private static readonly Type overridableMonoBehaviourType = typeof(NewMonoBehaviour);

    /// <summary>
    /// Update イベントに購読
    /// </summary>
    public static void SubscribeToUpdate(Action callback)
    {
        if (Instance == null) return;

        OnUpdateEvent += callback;
    }

    /// <summary>
    /// FixedUpdate イベントに購読
    /// </summary>
    public static void SubscribeToFixedUpdate(Action callback)
    {
        if (Instance == null) return;

        OnFixedUpdateEvent += callback;
    }

    /// <summary>
    /// LateUpdate イベントに購読
    /// </summary>
    public static void SubscribeToLateUpdate(Action callback)
    {
        if (Instance == null) return;

        OnLateUpdateEvent += callback;
    }

    /// <summary>
    /// Update イベントの購読をキャンセル
    /// </summary>
    public static void UnsubscribeFromUpdate(Action callback)
    {
        OnUpdateEvent -= callback;
    }

    /// <summary>
    /// FixedUpdate イベントの購読をキャンセル
    /// </summary>
    public static void UnsubscribeFromFixedUpdate(Action callback)
    {
        OnFixedUpdateEvent -= callback;
    }

    /// <summary>
    /// LateUpdate イベントの購読をキャンセル
    /// </summary>
    public static void UnsubscribeFromLateUpdate(Action callback)
    {
        OnLateUpdateEvent -= callback;
    }

    /// <summary>
    /// NewMonoBehaviour スクリプトをUpdateManagerに追加
    /// </summary>
    public static void AddItem(NewMonoBehaviour behaviour)
    {
        if (behaviour == null)
            throw new NullReferenceException("null!");

        // ゲームが終了している場合は追加しない
        if (isShuttingDown)
            return;

        AddItemToArray(behaviour);
    }

    /// <summary>
    /// 特定のNewMonoBehaviourスクリプトを削除
    /// </summary>
    public static void RemoveSpecificItem(NewMonoBehaviour behaviour)
    {
        if (behaviour == null)
            throw new NullReferenceException("null!");

        if (isShuttingDown)
            return;

        if (Instance != null) RemoveSpecificItemFromArray(behaviour);
    }

    /// <summary>
    /// 除去して破壊
    /// </summary>
    public static void RemoveSpecificItemAndDestroyComponent(NewMonoBehaviour behaviour)
    {
        if (behaviour == null)
            throw new NullReferenceException("null!");

        if (isShuttingDown)
            return;

        if (Instance != null)
            RemoveSpecificItemFromArray(behaviour);

        Destroy(behaviour);
    }

    /// <summary>
    /// 特定のNewMonoBehaviourをUpdateManagerの配列から削除し、オブジェクト破壊
    /// </summary>
    public static void RemoveSpecificItemAndDestroyGameObject(NewMonoBehaviour behaviour)
    {
        if (behaviour == null)
            throw new NullReferenceException("null!");

        if (isShuttingDown)
            return;

        if (Instance != null)
            RemoveSpecificItemFromArray(behaviour);

        Destroy(behaviour.gameObject);
    }

    /// <summary>
    /// 新しいNewMono BehaviourをUpdateManagerの配列に追加
    /// </summary>
    private static void AddItemToArray(NewMonoBehaviour behaviour)
    {
        Type behaviourType = behaviour.GetType();

        if (behaviourType.GetMethod("NewUpdate").DeclaringType != overridableMonoBehaviourType)
            SubscribeToUpdate(behaviour.NewUpdate);

        if (behaviourType.GetMethod("NewFixedUpdate").DeclaringType != overridableMonoBehaviourType)
            SubscribeToFixedUpdate(behaviour.NewFixedUpdate);

        if (behaviourType.GetMethod("NewLateUpdate").DeclaringType != overridableMonoBehaviourType)
            SubscribeToLateUpdate(behaviour.NewLateUpdate);
    }

    /// <summary>
    /// 特定のNewMonoBehaviourをUpdateManagerの配列から削除
    /// </summary>
    private static void RemoveSpecificItemFromArray(NewMonoBehaviour behaviour)
    {
        UnsubscribeFromUpdate(behaviour.NewUpdate);
        UnsubscribeFromFixedUpdate(behaviour.NewFixedUpdate);
        UnsubscribeFromLateUpdate(behaviour.NewLateUpdate);
    }

    /// <summary>
    /// 既存のUpdate()関数を代わりに呼び出してUpdateイベントを実行する関数
    /// </summary>
    private void Update()
    {
        if (OnUpdateEvent != null) OnUpdateEvent.Invoke();
    }
    
    /// <summary>
    /// 既存のFixedUpdate()関数を代わりに呼び出してFixedUpdateイベントを実行する関数
    /// </summary>
    private void FixedUpdate()
    {
        if (OnFixedUpdateEvent != null) OnFixedUpdateEvent.Invoke();
    }

    /// <summary>
    /// 既存のLateUpdate()関数を代わりに呼び出してLateUpdateイベントを実行する関数
    /// </summary>
    private void LateUpdate()
    {
        if (OnLateUpdateEvent != null) OnLateUpdateEvent.Invoke();
    }
}