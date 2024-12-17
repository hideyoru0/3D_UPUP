using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;   //UI 사용을 위해서 추가

public class Player : MonoBehaviour
{
    public float speed;
    public Follow follow; //스폰된 플레이어로 카메라 이동
    public GameManager gameManager;
    public GameObject UIRestartBtn;
    public Text UIStage;

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
        anim = GetComponentInChildren<Animator>();  //Player 자식 오브젝트에 있으므로
        follow = FindFirstObjectByType<Follow>(); //스폰된 플레이어로 카메라 이동
        gameManager = FindFirstObjectByType<GameManager>(); //게임 매니저 등록
    }

    void Start()
    {
        follow.target = this.transform;    //스폰된 플레이어로 카메라 이동
        gameManager.player = this;         //게임 매니저에 지정된 플레이어가 이 스크립트로 지정
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
        hAxis = Input.GetAxisRaw("Horizontal"); //좌우 방향키
        vAxis = Input.GetAxisRaw("Vertical");   //상하 방향키
        wDown = Input.GetButton("Walk");    //shift 키
        jDown = Input.GetButtonDown("Jump"); //스페이스바
    }

    void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;  //normalized : 방향 값이 1로 보정된 벡터

        transform.position += moveVec * speed * (wDown ? 0.3f : 1f) * Time.deltaTime;

        anim.SetBool("isRun", (moveVec != Vector3.zero));   //이동을 멈추면
        anim.SetBool("isWalk", wDown);
    }

    void Turn()
    {
        transform.LookAt(transform.position + moveVec); //나아갈 방향 보기
    }

    void Jump()
    {
        if (jDown && !isJump)
        {
            rigid.AddForce(Vector3.up * 15, ForceMode.Impulse);
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
        else if (collision.gameObject.tag == "DeadZone")
        {
            //재시작 버튼 UI
            gameManager.UIRestartBtn.SetActive(true);
            TextMeshProUGUI btnText = gameManager.UIRestartBtn.GetComponentInChildren<TextMeshProUGUI>();   //버튼 텍스트는 자식 오브젝트이므로 InChildren을 붙여야함
            //btnText.text = "Clear!";
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "SuperFloor")
        {
            anim.SetBool("isJump", false);
            isJump = false;
            //충돌시 튕겨짐
            //목표물 기준 왼쪽에서 닿으면 왼쪽으로, 오른쪽에서 닿으면 오른쪽으로
            OnJumpFloor();
        }
    }
    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Finish")
        {
            //다음 스테이지로 이동
            gameManager.NextStage();
        }
    }


    void OnJumpFloor()
    {   
        //충돌시 튕겨짐
        //목표물 기준 왼쪽에서 닿으면 왼쪽으로, 오른쪽에서 닿으면 오른쪽으로
        //int dirc = transform.position.y - targetPos.y > 0 ? 1 : -1;
        rigid.AddForce(Vector3.up * 10, ForceMode.Impulse);
    }

    public void VelocityZero()
    {
        rigid.velocity = Vector3.zero;
    }
}