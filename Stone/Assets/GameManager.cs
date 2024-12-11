using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Camera camera1; // �ŏ��̃J����
    public Camera camera2; // 2�ڂ̃J����

    public List<GameObject> SpawnedStones = new List<GameObject>();
    public bool OnSide = false;
    private void Start()
    {
        // �ŏ���camera1��L�����Acamera2�𖳌���
        camera1.gameObject.SetActive(true);
        camera2.gameObject.SetActive(false);
    }

    private void Update()
    {
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
}
