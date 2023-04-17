using UnityEngine;

public abstract class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    protected abstract bool DoNotDestroyOnLoad { get; }

    /// <summary>
    /// アプリケーションが、終了したかどうか
    /// </summary>
    protected static bool isShuttingDown = false;
    /// <summary>
    /// T型のインスタンスを格納する変数
    /// </summary>
    private static T instance;
    /// <summary>
    /// 同時性の問題を解決するためのobjectLock変数
    /// </summary>
    private static object objectLock;
    /// <summary>
    /// T型のType情報
    /// </summary>
    private static readonly System.Type instanceType = typeof(T);
    
    /// <summary>
    /// インスタンスを取得
    /// </summary>
    public static T Instance
    {
        get
        {
            // 終了中はnullを返す
            if (isShuttingDown)
                return null;
            
            // 同時性の問題を解決するためのlock
            if (objectLock == null)
                objectLock = new object();

            lock (objectLock)
            {
                // インスタンスが既にある場合はそのインスタンスを返す
                if (instance != null) return instance;
                // シーンからインスタンスを検索
                instance = (T)FindObjectOfType(instanceType);
                
                // シーンからインスタンスを検索できなかった場合
                if (instance != null) 
                    return instance;
                
                instance = (T)new GameObject(instanceType.Name).AddComponent(instanceType);
                // DoNotDestroyOnLoadがtrueの場合、シーンが変わっても破棄されない
                SingletonBehaviour<T> singleton = instance as SingletonBehaviour<T>;
                if (singleton != null && singleton.DoNotDestroyOnLoad) DontDestroyOnLoad(instance);

                return instance;
            }
        }
    }

    /// <summary>
    /// アプリケーションが終了するとき
    /// </summary>
    private void OnApplicationQuit() => isShuttingDown = true;

    /// <summary>
    /// オブジェクトが破棄されるとき
    /// </summary>
    private void OnDestroy() => isShuttingDown = true;
}