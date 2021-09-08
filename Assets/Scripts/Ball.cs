using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��ϰ� �ε����� �� ����� ��Ȱ��ȭ �ϰ� �ʹ�.
public class Ball : MonoBehaviour
{
    public PlayerAgent player;
    // ��ϰ� �ε����� �� ����� ��Ȱ��ȭ �ϰ� �ʹ�.
    private void OnCollisionEnter(Collision collision)
    {
        // 1. �ε��� �༮�� ������� Ȯ���ؾ� �Ѵ�.
        if (collision.gameObject.name.Contains("Block"))
        {
            // PlayerAgent �� �ִ� OnBlockHit �Լ��� ȣ���ϰ� �ʹ�.
            player.OnBlockHit(collision.gameObject);
            // 2. ����� ��Ȱ��ȭ �ϰ� �ʹ�.
            //collision.gameObject.SetActive(false);
        }
    }
}
