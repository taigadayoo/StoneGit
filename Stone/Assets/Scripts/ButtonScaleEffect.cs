using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class ButtonScaleEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 originalScale; // 元の大きさ
    public float scaleFactor = 1.2f; // 拡大倍率

    void Start()
    {
        originalScale = transform.localScale; // ボタンの元のスケールを保存
    }
    private void Update()
    {
        if (!IsMouseOverObject())
        {
            transform.localScale = originalScale; // マウスがオブジェクトに触れていない場合は元に戻す
        }
    }
    // マウスが重なったとき
    public void OnPointerEnter(PointerEventData eventData)
    {
        SoundManager.Instance.PlaySe(SeType.SE1);
        transform.localScale = originalScale * scaleFactor; // 拡大
    }

    // マウスが離れたとき
    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = originalScale; // 元に戻す
    }
    private bool IsMouseOverObject()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        return rectTransform.rect.Contains(rectTransform.InverseTransformPoint(Input.mousePosition));
    }
}