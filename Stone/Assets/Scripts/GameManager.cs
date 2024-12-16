using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public Camera camera1; // �ŏ��̃J����
    public Camera camera2; // 2�ڂ̃J����
    private float _cameraY;
    float smoothSpeed = 5f; // �X���[�Y�Ȉړ����x�i�傫���قǑ��������j
    [SerializeField]
    StoneSpawner stoneSpawner;
    public List<GameObject> SpawnedStones = new List<GameObject>();
    public List<Rigidbody> rigidbodyList = new List<Rigidbody>();
    public bool OnSide = false;
    public bool IsGameOver = false;
    public RectTransform targetRectTransform; // ����������RectTransform
    [SerializeField]
    Vector3 targetPosition = new Vector3(0, 0, 0);
    [SerializeField]
    float duration = 1;
    Timer timer;
    private void Start()
    {
        timer = FindObjectOfType<Timer>();
        // �ŏ���camera1��L�����Acamera2�𖳌���
        camera1.gameObject.SetActive(true);
        camera2.gameObject.SetActive(false);
      
    }

    private void Update()
    {
 

        if (stoneSpawner.highestY >= 0.5f)
        {
            _cameraY = stoneSpawner.highestY;

            Vector3 targetPos1 = new Vector3(camera1.transform.position.x, _cameraY, camera1.transform.position.z);
            Vector3 targetPos2 = new Vector3(camera2.transform.position.x, _cameraY, camera2.transform.position.z);

            // �X���[�Y�Ƀ^�[�Q�b�g�ʒu�ֈړ�
            camera1.transform.position = Vector3.Lerp(camera1.transform.position, targetPos1, smoothSpeed * Time.deltaTime);
            camera2.transform.position = Vector3.Lerp(camera2.transform.position, targetPos2, smoothSpeed * Time.deltaTime);
        }
        // �X�y�[�X�L�[�ŃJ������؂�ւ�
        if (Input.GetKeyDown(KeyCode.R))
        {
            // ���݂̃J�����̏�Ԃ�؂�ւ���
            bool isCamera1Active = camera1.gameObject.activeSelf;

            camera1.gameObject.SetActive(!isCamera1Active);
            camera2.gameObject.SetActive(isCamera1Active);

            OnSide = isCamera1Active;
        }
    }
    public void AddRigidbody()
    {
        foreach (GameObject obj in SpawnedStones)
        {
            // Rigidbody�R���|�[�l���g���擾
            Rigidbody rb = obj.GetComponent<Rigidbody>();

            // Rigidbody�����݂���ꍇ�A���X�g�ɒǉ�
            if (rb != null)
            {
                rigidbodyList.Add(rb);
            }
        }
    }
    public void GameOver()
    {
        IsGameOver = true;
        targetRectTransform.DOAnchorPos(targetPosition, duration).SetEase(Ease.OutCubic);
        timer.timerText.gameObject.SetActive(false);
        stoneSpawner.highText.gameObject.SetActive(false);
    }
  public  void SetAllRigidbodiesKinematic(bool isKinematic)
    {
        foreach (Rigidbody rb in rigidbodyList)
        {
            rb.isKinematic = isKinematic;
        }
    }
}
