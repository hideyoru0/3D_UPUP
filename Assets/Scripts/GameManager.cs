using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   //UI ����� ���ؼ� �߰�
using UnityEngine.SceneManagement;   //Scene ��ȯ�� ���ؼ� �߰�


public class GameManager : MonoBehaviour
{
    public int stageIndex;  //�������� ��ȣ
    public GameObject[] Stages;
    public Player player;
    public Text UIStage;
    public GameObject UIRestartBtn;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PlayerReposition()
    {   //�÷��̾� ��ġ �ǵ����� �Լ�
        player.transform.position = new Vector3(-5.0f, 5.0f, 5.0f);  //�÷��̾� ��ġ �̵�
        player.VelocityZero();  //�÷��̾� ���� �ӵ� 0���� �����
    }

    public void NextStage()
    {
        //���� ���������� �̵�
        if (stageIndex < Stages.Length - 1)
        {
            Stages[stageIndex].SetActive(false);    //���� �������� ��Ȱ��ȭ
            stageIndex++;
            Stages[stageIndex].SetActive(true); //���� �������� Ȱ��ȭ
            PlayerReposition();

            UIStage.text = "STAGE " + (stageIndex + 1);
        }
        else
        {  //���� Ŭ�����
            //���߱�
            //Time.timeScale = 0;

            //����� ��ư UI
            UIRestartBtn.SetActive(true);
            Text btnText = UIRestartBtn.GetComponentInChildren<Text>();   //��ư �ؽ�Ʈ�� �ڽ� ������Ʈ�̹Ƿ� InChildren�� �ٿ�����
            btnText.text = "Clear!";
            UIRestartBtn.SetActive(true);
        }
    }

    public void Restart()
    {
        //Time.timeScale = 1; //����۽� �ð� ����
        SceneManager.LoadScene(0);
    }
}
