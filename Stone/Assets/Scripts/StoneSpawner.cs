using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StoneSpawner : MonoBehaviour
{
    // �X�|�[��������Q�[���I�u�W�F�N�g�̃��X�g
    public List<GameObject> objectsToSpawn;

    public GameObject deadCol;
    // �X�|�[����̈ʒu
    public Transform spawnPosition;
    GameManager gameManager;
    // �t���O�i�����ɍ��킹�ďグ��j
    private bool onSpawn = false;
    public float highestY = 0;
    public bool OnStone = false;
    public Text highText;
    public Text NowScore;
    [SerializeField]
    DemonIEvent demonEvent;
    public float highTextNum;
    void Start()
    {
        highestY = Mathf.Floor(highestY * 10 * 200) / 10; // �����l�𖾎��I�ɐ؂�̂�
        highText.text = $"{highestY + 96}����";
        // ����̃����_���X�|�[��
        SpawnRandomObject();
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        if (!gameManager.IsGameOver)
        {
            
            highestY = GetHighestYValue(gameManager.SpawnedStones);
            float highestYMathf = Mathf.Round(highestY * 1000f) / 1000f;
            if (OnStone)
            {
                AlignTargetToHighestY(this.gameObject, highestYMathf + 0.17f);
                Vector3 DeadColPos = new Vector3(deadCol.transform.position.x, highestY - 0.25f, deadCol.transform.position.z);
                deadCol.transform.position = DeadColPos;
            }
            NowScore.text = highText.text;
            highTextNum = Mathf.Floor(highestY * 3000) / 10;
            if (highTextNum <= 0)
            {
                highTextNum = 0;
            }
            highText.text = $"{highTextNum }cm";

           
            // �t���O���オ�����Ƃ��Ƀ����_���X�|�[��
            if (onSpawn)
            {
                //demonEvent.EventCall();
                SpawnRandomObject();
                onSpawn = false; // �t���O�����Z�b�g
            }
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