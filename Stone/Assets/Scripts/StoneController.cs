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
        float moveInput = Input.GetAxisRaw("Horizontal"); // A/D �L�[�܂��͍�/�E���L�[

        // �I�u�W�F�N�g�̉E�����i���[�J�����W�n�ł̉E�����j���擾

        // ���������Ɉړ�
        Vector3 moveDirection;
        if (gameManager.OnSide)
        {
            // ��O�i�J���������j/�������i�J�����Ƌt�����j�Ɉړ�
            moveDirection = new Vector3(0f, 0f, moveInput * -moveSpeed * Time.deltaTime);
        }
        else
        {
            // ���E�����Ɉړ�
            moveDirection = new Vector3(moveInput * -moveSpeed * Time.deltaTime, 0f, 0f);
        }

        // ���݂̈ʒu�Ɉړ��ʂ����Z
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
