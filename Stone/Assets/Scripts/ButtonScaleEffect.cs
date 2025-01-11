using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class ButtonScaleEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 originalScale; // ���̑傫��
    public float scaleFactor = 1.2f; // �g��{��

    void Start()
    {
        originalScale = transform.localScale; // �{�^���̌��̃X�P�[����ۑ�
    }
    private void Update()
    {
        if (!IsMouseOverObject())
        {
            transform.localScale = originalScale; // �}�E�X���I�u�W�F�N�g�ɐG��Ă��Ȃ��ꍇ�͌��ɖ߂�
        }
    }
    // �}�E�X���d�Ȃ����Ƃ�
    public void OnPointerEnter(PointerEventData eventData)
    {
        SoundManager.Instance.PlaySe(SeType.SE1);
        transform.localScale = originalScale * scaleFactor; // �g��
    }

    // �}�E�X�����ꂽ�Ƃ�
    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = originalScale; // ���ɖ߂�
    }
    private bool IsMouseOverObject()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        return rectTransform.rect.Contains(rectTransform.InverseTransformPoint(Input.mousePosition));
    }
}