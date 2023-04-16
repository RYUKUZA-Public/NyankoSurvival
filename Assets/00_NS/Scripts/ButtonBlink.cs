using UnityEngine;
using UnityEngine.UI;

public class ButtonBlink : MonoBehaviour
{
    /// <summary>
    /// 点滅対象画像
    /// </summary>
    private Image blinkImage;

    /// <summary>
    /// 点滅色配列
    /// </summary>
    public Color[] outlineColors;

    /// <summary>
    /// 現在の色
    /// </summary>
    private int currentIndex = 0;

    /// <summary>
    /// 色変更周期 (秒)
    /// </summary>
    public float changeInterval = 0.1f;

    // 次の色変更時間
    private float nextChangeTime = 0f;
    
    void Start()
    {
        blinkImage = GetComponent<Image>();
        blinkImage.color = outlineColors[currentIndex];
    }
    
    void Update()
    {
        // 変更周期ごとに色変更
        if (Time.time > nextChangeTime)
        {
            currentIndex++;
            if (currentIndex >= outlineColors.Length)
            {
                currentIndex = 0;
            }

            blinkImage.GetComponent<Image>().color = outlineColors[currentIndex];
            nextChangeTime = Time.time + changeInterval;
        }
    }
}