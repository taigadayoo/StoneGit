using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneSpawner : MonoBehaviour
{
    // �X�|�[��������Q�[���I�u�W�F�N�g�̃��X�g
    public List<GameObject> objectsToSpawn;

    // �X�|�[����̈ʒu
    public Transform spawnPosition;
    GameManager gameManager;
    // �t���O�i�����ɍ��킹�ďグ��j
    private bool onSpawn = false;
    private float highestY;
    void Start()
    {
        // ����̃����_���X�|�[��
        SpawnRandomObject();
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        highestY = GetHighestYValue(gameManager.SpawnedStones);


        AlignTargetToHighestY(this.gameObject, highestY+0.15f);
        // �t���O���オ�����Ƃ��Ƀ����_���X�|�[��
        if (onSpawn)
        {
            SpawnRandomObject();
            onSpawn = false; // �t���O�����Z�b�g
        }
    }

    void SpawnRandomObject()
    {
        // ���X�g����łȂ����m�F
        if (objectsToSpawn.Count > 0)
        {
            // �����_���ȃC���f�b�N�X��I��
            int randomIndex = Random.Range(0, objectsToSpawn.Count);

            // ���X�g���烉���_���ɃI�u�W�F�N�g���擾
            GameObject randomObject = objectsToSpawn[randomIndex];

            // �V�����ʒu�ɃI�u�W�F�N�g���X�|�[��
            Instantiate(randomObject, spawnPosition.position, randomObject.transform.rotation);
        }
    }
    public void StartRespawn(bool value)
    {
        StartCoroutine(SpawnStone(value));
    }
    // �t���O���O������ύX���郁�\�b�h
    public IEnumerator SpawnStone(bool value)
    {
        yield return new WaitForSeconds(1f);

        onSpawn = value;
    }
    private float GetHighestYValue(List<GameObject> objList)
    {
        float maxY = float.MinValue; // �����l����ɒႢ�l�ɐݒ�

        foreach (GameObject obj in objList)
        {
            if (obj != null) // �I�u�W�F�N�g�����݂���ꍇ�̂ݏ���
            {
                float currentY = obj.transform.position.y;
                if (currentY > maxY)
                {
                    maxY = currentY;
                }
            }
        }

        return maxY;
    }
    private void AlignTargetToHighestY(GameObject target, float newY)
    {
        Vector3 currentPosition = target.transform.position;
        target.transform.position = new Vector3(currentPosition.x, newY, currentPosition.z);
    }
}