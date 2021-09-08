using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 1. 사용자의 입력에따라 좌우로 이동하고 싶다.
// 필요속성 : 이동속도
// 2. Ball 을 특정 방향으로 발사하고 싶다.
// 필요속성 : 특정방향, 발사힘
public class PlayerAgent : MonoBehaviour
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

    void Start()
    {
        // ballRB 에 값을 할당하자
        // Ball 에 붙어 있는 Rigidbody 컴포넌트를 얻어와서 할당.
        // Ball 이 필요하다.
        ballRB = ball.GetComponent<Rigidbody>();
        ShootBall();
    }

    // Update is called once per frame
    void Update()
    {
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
        // 최종 방향값은 h - 1
        h = h - 1;

        // 2. 방향이 필요
        // (x, y, z) -> (1, 0, 0)
        Vector3 v = Vector3.right * h  * speed;
        //Vector3 v = new Vector3(h, 0, 0);
        Vector3 vt = v * Time.deltaTime;
        // 3. 이동하고 싶다.
        // P = P0 + vt
        Vector3 P0 = transform.localPosition;
        Vector3 P = P0 + vt;

        transform.localPosition = P;
    }
}
