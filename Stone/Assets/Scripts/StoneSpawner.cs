using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StoneSpawner : MonoBehaviour
{
    // スポーンさせるゲームオブジェクトのリスト
    public List<GameObject> objectsToSpawn;

    public GameObject deadCol;
    // スポーン先の位置
    public Transform spawnPosition;
    GameManager gameManager;
    // フラグ（条件に合わせて上げる）
    private bool onSpawn = false;
    public float highestY = 0;
    public Text highText;
    void Start()
    {
        highestY = Mathf.Floor(highestY * 10 * 200) / 10; // 初期値を明示的に切り捨て
        highText.text = $"{highestY + 96}ｃｍ";
        // 初回のランダムスポーン
        SpawnRandomObject();
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        highestY = GetHighestYValue(gameManager.SpawnedStones);
        Vector3 DeadColPos = new Vector3(deadCol.transform.position.x, highestY - 0.15f, deadCol.transform.position.z);
        deadCol.transform.position = DeadColPos;
        float highTextNum = Mathf.Floor(highestY* 3000) / 10;
        if(highTextNum <= 0)
        {
            highTextNum = 0;
        }
        highText.text = $"{highTextNum }ｃｍ";

        AlignTargetToHighestY(this.gameObject, highestY+0.15f);
        // フラグが上がったときにランダムスポーン
        if (onSpawn)
        {
            SpawnRandomObject();
            onSpawn = false; // フラグをリセット
        }
    }

    void SpawnRandomObject()
    {
        // リストが空でないか確認
        if (objectsToSpawn.Count > 0)
        {
            // ランダムなインデックスを選択
            int randomIndex = Random.Range(0, objectsToSpawn.Count);

            // リストからランダムにオブジェクトを取得
            GameObject randomObject = objectsToSpawn[randomIndex];

            // 新しい位置にオブジェクトをスポーン
            Instantiate(randomObject, spawnPosition.position, randomObject.transform.rotation);
        }
    }
    public void StartRespawn(bool value)
    {
        StartCoroutine(SpawnStone(value));
    }
    // フラグを外部から変更するメソッド
    public IEnumerator SpawnStone(bool value)
    {
        yield return new WaitForSeconds(1f);

        onSpawn = value;
    }
    private float GetHighestYValue(List<GameObject> objList)
    {
        float maxY = float.MinValue; // 初期値を非常に低い値に設定

        foreach (GameObject obj in objList)
        {
            if (obj != null) // オブジェクトが存在する場合のみ処理
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