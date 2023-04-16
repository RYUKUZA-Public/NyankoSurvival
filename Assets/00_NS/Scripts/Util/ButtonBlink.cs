using System.Collections;
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
    [SerializeField]
    private Color[] outlineColors;
    
    /// <summary>
    /// 色変更周期 (秒)
    /// </summary>
    [SerializeField]
    private float blinkSpeed = 0.1f;
    
    void Start()
    {
        blinkImage = GetComponent<Image>();
        StartCoroutine(Blink());
    }
    
    private IEnumerator Blink()
    {
        int index = 0;
        while (true)
        {
            blinkImage.color = outlineColors[index];
            index = (index + 1) % outlineColors.Length;
            yield return new WaitForSecondsRealtime(blinkSpeed);
        }
    }
}