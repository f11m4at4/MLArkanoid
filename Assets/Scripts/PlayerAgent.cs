using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

// 1. ������� �Է¿����� �¿�� �̵��ϰ� �ʹ�.
// �ʿ�Ӽ� : �̵��ӵ�
// 2. Ball �� Ư�� �������� �߻��ϰ� �ʹ�.
// �ʿ�Ӽ� : Ư������, �߻���
// 3. Ball �� Player ������ �������� �ٽ� �����ϰ� ����
// ������� : Ball �� Player �� ������� ���������Ѵ�. 
// �ʿ�Ӽ� : Ball, Player �� �ʱ���ġ
// 4. ����� �� ������ �ٽ� �����ϰ� �ϰ� �ʹ�.
public class PlayerAgent : Agent
{
    // �ʿ�Ӽ� : �̵��ӵ�
    public float speed = 5;

    // -- Ball �Ӽ�
    // �ʿ�Ӽ� : Ư������, �߻���
    [SerializeField]
    int shootAngle = 45;
    [SerializeField]
    int shootPower = 5;

    Rigidbody ballRB;
    public Transform ball;
    // �ʿ�Ӽ� : Ball, Player �� �ʱ���ġ
    Vector3 ballInitPos;
    Vector3 playerInitPos;

    // ��� ��ϵ�
    //public GameObject []blocks;
    public GameObject blocks;

    // ��ü ��� ����
    int totalBlocks;
    // ��� ������ ����� ����
    int breakCount;

    // 2. Ball �� Ư�� �������� �߻��ϰ� �ʹ�.
    void ShootBall()
    {
        // Ball �� Ư�� �������� �߻��ϰ� �ʹ�.
        // 1. ������ �ʿ�
        //  1 - 1 ������ �ʿ��ϴ�.
        int angle = Random.Range(-shootAngle, shootAngle);
        //  1 - 2 �� ������ ���� ���͸� ������
        Vector3 velocity =  Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.up;
        // 2. �߻��ϰ�ʹ�.
        ballRB.velocity = velocity * shootPower;
    }

    private void Reset()
    {
        breakCount = 0;
        ball.localPosition = ballInitPos;
        transform.localPosition = playerInitPos;

        // ball �� �ӵ��� �ʱ�ȭ
        ballRB.velocity = Vector3.zero;

        // ��� ����� �ٽ� Ȱ��ȭ ������� �Ѵ�.
        // 1. ��� ����� ����ϰ� �ִٰ� Ȱ��ȭ ��������
        //for(int i=0;i< blocks.Length;i++)
        //{
        //    blocks[i].SetActive(true);
        //}
        
        // 2. Blocks �� �ڽ��� ��� �����ͼ� Ȱ��ȭ ��������
        foreach(Transform block in blocks.transform)
        {
            block.gameObject.SetActive(true);
        }

        // Ball �ٽ� �߻��ϰ� �ʹ�.
        // �����ð� ���� �����ؼ� �߻��ϰ� �ʹ�.
        Invoke("ShootBall", 1);
    }

    public override void Initialize()
    {
        // ��ü���� �ʱⰪ ���
        ballInitPos = ball.localPosition;
        playerInitPos = transform.localPosition;
        // ballRB �� ���� �Ҵ�����
        // Ball �� �پ� �ִ� Rigidbody ������Ʈ�� ���ͼ� �Ҵ�.
        // Ball �� �ʿ��ϴ�.
        ballRB = ball.GetComponent<Rigidbody>();

        // ��ü ��� ���� ���
        totalBlocks = blocks.transform.childCount;

    }

    // ���� �����Ҷ� ȣ�� �Լ�(�ݹ�)
    public override void OnEpisodeBegin()
    {
        Reset(); 
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        // ��� �����̵��� �������� 
        AddReward(-1.0f / MaxStep);
        float dir = actions.DiscreteActions.Array[0];
        Move(dir);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // 1. ������ġ
        sensor.AddObservation(ball.localPosition);
        // 2. �÷��̾��� ��ġ
        sensor.AddObservation(transform.localPosition);
        // 3. ���� �÷��̾��� �Ÿ�
        float distance = Vector3.Distance(ball.localPosition, transform.localPosition);
        sensor.AddObservation(distance);
    }

    // �׽�Ʈ�� ���� ����� ���� action ���� ������ �ִ� ���
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        // ���� ġƮŰ ������̶��
        if (isTest && Input.GetButtonDown("Fire1"))
        {
            // ����� ���ʴ�� �ϳ��� ���� �ʹ�.
            OnBlockHit(blocks.transform.GetChild(breakCount).gameObject);
        }
        // ������� �Է¿����� �¿�� �̵��ϰ� �ʹ�.
        // 1. ����� �Է¿�����
        float h = 1;

        // ���� ����ڰ� A Ű�� �����ٸ�
        if (Input.GetKey(KeyCode.A))
        {
            // => h �� 0 ���� ����
            h = 0;
        }
        // ���� ����ڰ� D Ű�� �����ٸ�
        else if (Input.GetKey(KeyCode.D))
        {
            // => h �� 2 �� ����
            h = 2;
        }

        actionsOut.DiscreteActions.Array[0] = (int)h;
    }

    // �׽�Ʈ�� ���� ġƮŰ
    public bool isTest = false;


    private void Move(float h)
    {
        // ���� ���Ⱚ�� h - 1
        h = h - 1;

        // 2. ������ �ʿ�
        // (x, y, z) -> (1, 0, 0)
        Vector3 v = Vector3.right * h * speed;
        //Vector3 v = new Vector3(h, 0, 0);
        Vector3 vt = v * Time.deltaTime;
        // 3. �̵��ϰ� �ʹ�.
        // P = P0 + vt
        Vector3 P0 = transform.localPosition;
        Vector3 P = P0 + vt;

        transform.localPosition = P;

        // Ball �� Player ������ �������� �ٽ� �����ϰ� ����
        // 1. ���� Ball �� y ���� Player �� y ������ �� �۾����ٸ�
        if (ball.localPosition.y < transform.localPosition.y)
        {
            // 2. �ٽ� �����ϰ� �ϰ� �ʹ�.
            // �г�Ƽ ����
            AddReward(-5);
            // OnEpisodeBegin() �� ȣ��
            EndEpisode();
        }
    }

    public void OnBlockHit(GameObject hitBlock)
    {
        // Ī��
        AddReward(0.5f);
        hitBlock.SetActive(false);
        breakCount++;
        // ���� �� ���ٸ�
        // ���� breakCount �� ��ü ��ϰ��� �̻��̶��
        if(breakCount >= totalBlocks)
        {
            AddReward(10);
            EndEpisode();
        }
    }
}
