using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public Follow follow; //������ �÷��̾�� ī�޶� �̵�

    float hAxis;
    float vAxis;
    bool wDown;
    bool jDown;
    bool isJump;

    Vector3 moveVec;

    Animator anim;
    Rigidbody rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();  //Player �ڽ� ������Ʈ�� �����Ƿ�
        follow = FindFirstObjectByType<Follow>(); //������ �÷��̾�� ī�޶� �̵�
    }

    void Start()
    {
        follow.target = this.transform;    //������ �÷��̾�� ī�޶� �̵�
    }

    void Update()
    {
        GetInput();
        Move();
        Turn();
        Jump();
    }

    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal"); //�¿� ����Ű
        vAxis = Input.GetAxisRaw("Vertical");   //���� ����Ű
        wDown = Input.GetButton("Walk");    //shift Ű
        jDown = Input.GetButtonDown("Jump"); //�����̽���
    }

    void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;  //normalized : ���� ���� 1�� ������ ����

        transform.position += moveVec * speed * (wDown ? 0.3f : 1f) * Time.deltaTime;

        anim.SetBool("isRun", (moveVec != Vector3.zero));   //�̵��� ���߸�
        anim.SetBool("isWalk", wDown);
    }

    void Turn()
    {
        transform.LookAt(transform.position + moveVec); //���ư� ���� ����
    }

    void Jump()
    {
        if (jDown && !isJump)
        {
            rigid.AddForce(Vector3.up * 10, ForceMode.Impulse);
            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");
            isJump = true;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            anim.SetBool("isJump", false);
            isJump = false;
        }
    }
}