using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 1. ������� �Է¿����� �¿�� �̵��ϰ� �ʹ�.
// �ʿ�Ӽ� : �̵��ӵ�
// 2. Ball �� Ư�� �������� �߻��ϰ� �ʹ�.
// �ʿ�Ӽ� : Ư������, �߻���
public class PlayerAgent : MonoBehaviour
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

    void Start()
    {
        // ballRB �� ���� �Ҵ�����
        // Ball �� �پ� �ִ� Rigidbody ������Ʈ�� ���ͼ� �Ҵ�.
        // Ball �� �ʿ��ϴ�.
        ballRB = ball.GetComponent<Rigidbody>();
        ShootBall();
    }

    // Update is called once per frame
    void Update()
    {
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
        // ���� ���Ⱚ�� h - 1
        h = h - 1;

        // 2. ������ �ʿ�
        // (x, y, z) -> (1, 0, 0)
        Vector3 v = Vector3.right * h  * speed;
        //Vector3 v = new Vector3(h, 0, 0);
        Vector3 vt = v * Time.deltaTime;
        // 3. �̵��ϰ� �ʹ�.
        // P = P0 + vt
        Vector3 P0 = transform.localPosition;
        Vector3 P = P0 + vt;

        transform.localPosition = P;
    }
}
