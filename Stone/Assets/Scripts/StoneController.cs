using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneController : MonoBehaviour
{
    public float moveSpeed = 5f;
    // Start is called before the first frame update
    Rigidbody rb;
    StoneSpawner stoneSpawner;
   public Collider col1;
   public Collider col2;
    GameManager gameManager;
    void Start()
    {
       
        col1.enabled = false;
        col2.enabled = false;
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        stoneSpawner = FindObjectOfType<StoneSpawner>();
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        float moveInput = Input.GetAxisRaw("Horizontal"); // A/D キーまたは左/右矢印キー

        // オブジェクトの右方向（ローカル座標系での右方向）を取得

        // 水平方向に移動
        Vector3 moveDirection;
        if (gameManager.OnSide)
        {
            // 手前（カメラ方向）/奥方向（カメラと逆方向）に移動
            moveDirection = new Vector3(0f, 0f, moveInput * -moveSpeed * Time.deltaTime);
        }
        else
        {
            // 左右方向に移動
            moveDirection = new Vector3(moveInput * -moveSpeed * Time.deltaTime, 0f, 0f);
        }

        // 現在の位置に移動量を加算
        transform.position += moveDirection;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            col1.enabled = true;
            col2.enabled = true;
            rb.isKinematic = false;
            gameManager.SpawnedStones.Add(this.gameObject);
            stoneSpawner.StartRespawn(true);
            this.enabled = false;

        }
    }
}
