using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������� �Է¿����� �¿�� �̵��ϰ� �ʹ�.
// �ʿ�Ӽ� : �̵��ӵ�
public class PlayerAgent : MonoBehaviour
{
    // �ʿ�Ӽ� : �̵��ӵ�
    public float speed = 5;
    
    void Start()
    {

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
