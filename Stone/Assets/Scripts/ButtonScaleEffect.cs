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
}