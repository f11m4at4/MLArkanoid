using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

// 1. 사용자의 입력에따라 좌우로 이동하고 싶다.
// 필요속성 : 이동속도
// 2. Ball 을 특정 방향으로 발사하고 싶다.
// 필요속성 : 특정방향, 발사힘
// 3. Ball 이 Player 밑으로 떨어지면 다시 시작하게 하자
// 고려사항 : Ball 과 Player 을 원래대로 돌려놔야한다. 
// 필요속성 : Ball, Player 의 초기위치
// 4. 블록을 다 깼으면 다시 시작하게 하고 싶다.
public class PlayerAgent : Agent
{
    // 필요속성 : 이동속도
    public float speed = 5;

    // -- Ball 속성
    // 필요속성 : 특정방향, 발사힘
    [SerializeField]
    int shootAngle = 45;
    [SerializeField]
    int shootPower = 5;

    Rigidbody ballRB;
    public Transform ball;
    // 필요속성 : Ball, Player 의 초기위치
    Vector3 ballInitPos;
    Vector3 playerInitPos;

    // 모든 블록들
    //public GameObject []blocks;
    public GameObject blocks;

    // 전체 블록 갯수
    int totalBlocks;
    // 몇개나 깼는지 기억할 변수
    int breakCount;

    // 2. Ball 을 특정 방향으로 발사하고 싶다.
    void ShootBall()
    {
        // Ball 을 특정 방향으로 발사하고 싶다.
        // 1. 방향이 필요
        //  1 - 1 각도가 필요하다.
        int angle = Random.Range(-shootAngle, shootAngle);
        //  1 - 2 그 각도로 방향 벡터를 구하자
        Vector3 velocity =  Quaternion.AngleAxis(angle, Vector3.forward) * Vector3.up;
        // 2. 발사하고싶다.
        ballRB.velocity = velocity * shootPower;
    }

    private void Reset()
    {
        breakCount = 0;
        ball.localPosition = ballInitPos;
        transform.localPosition = playerInitPos;

        // ball 의 속도도 초기화
        ballRB.velocity = Vector3.zero;

        // 모든 블록을 다시 활성화 시켜줘야 한다.
        // 1. 모든 블록을 기억하고 있다가 활성화 시켜주자
        //for(int i=0;i< blocks.Length;i++)
        //{
        //    blocks[i].SetActive(true);
        //}
        
        // 2. Blocks 의 자식을 모두 가져와서 활성화 시켜주자
        foreach(Transform block in blocks.transform)
        {
            block.gameObject.SetActive(true);
        }

        // Ball 다시 발사하고 싶다.
        // 일정시간 동안 지연해서 발사하고 싶다.
        Invoke("ShootBall", 1);
    }

    public override void Initialize()
    {
        // 객체들의 초기값 기억
        ballInitPos = ball.localPosition;
        playerInitPos = transform.localPosition;
        // ballRB 에 값을 할당하자
        // Ball 에 붙어 있는 Rigidbody 컴포넌트를 얻어와서 할당.
        // Ball 이 필요하다.
        ballRB = ball.GetComponent<Rigidbody>();

        // 전체 블록 갯수 기억
        totalBlocks = blocks.transform.childCount;

    }

    // 판을 시작할때 호출 함수(콜백)
    public override void OnEpisodeBegin()
    {
        Reset(); 
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        // 계속 움직이도록 재촉하자 
        AddReward(-1.0f / MaxStep);
        float dir = actions.DiscreteActions.Array[0];
        Move(dir);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // 1. 공의위치
        sensor.AddObservation(ball.localPosition);
        // 2. 플레이어의 위치
        sensor.AddObservation(transform.localPosition);
        // 3. 공과 플레이어의 거리
        float distance = Vector3.Distance(ball.localPosition, transform.localPosition);
        sensor.AddObservation(distance);
    }

    // 테스트를 위해 사람이 직접 action 값을 전달해 주는 기능
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        // 만약 치트키 사용중이라면
        if (isTest && Input.GetButtonDown("Fire1"))
        {
            // 블록을 차례대로 하나씩 깨고 싶다.
            OnBlockHit(blocks.transform.GetChild(breakCount).gameObject);
        }
        // 사용자의 입력에따라 좌우로 이동하고 싶다.
        // 1. 사용자 입력에따라
        float h = 1;

        // 만약 사용자가 A 키를 눌렀다면
        if (Input.GetKey(KeyCode.A))
        {
            // => h 를 0 으로 놓자
            h = 0;
        }
        // 만약 사용자가 D 키를 눌렀다면
        else if (Input.GetKey(KeyCode.D))
        {
            // => h 를 2 로 놓자
            h = 2;
        }

        actionsOut.DiscreteActions.Array[0] = (int)h;
    }

    // 테스트를 위한 치트키
    public bool isTest = false;


    private void Move(float h)
    {
        // 최종 방향값은 h - 1
        h = h - 1;

        // 2. 방향이 필요
        // (x, y, z) -> (1, 0, 0)
        Vector3 v = Vector3.right * h * speed;
        //Vector3 v = new Vector3(h, 0, 0);
        Vector3 vt = v * Time.deltaTime;
        // 3. 이동하고 싶다.
        // P = P0 + vt
        Vector3 P0 = transform.localPosition;
        Vector3 P = P0 + vt;

        transform.localPosition = P;

        // Ball 이 Player 밑으로 떨어지면 다시 시작하게 하자
        // 1. 만약 Ball 의 y 값이 Player 의 y 값보다 더 작아졌다면
        if (ball.localPosition.y < transform.localPosition.y)
        {
            // 2. 다시 시작하게 하고 싶다.
            // 패널티 보상
            AddReward(-5);
            // OnEpisodeBegin() 을 호출
            EndEpisode();
        }
    }

    public void OnBlockHit(GameObject hitBlock)
    {
        // 칭찬
        AddReward(0.5f);
        hitBlock.SetActive(false);
        breakCount++;
        // 만약 다 깼다면
        // 만약 breakCount 가 전체 블록갯수 이상이라면
        if(breakCount >= totalBlocks)
        {
            AddReward(10);
            EndEpisode();
        }
    }
}
