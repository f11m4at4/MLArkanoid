using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 블록과 부딪혔을 때 블록을 비활성화 하고 싶다.
public class Ball : MonoBehaviour
{
    public PlayerAgent player;
    // 블록과 부딪혔을 때 블록을 비활성화 하고 싶다.
    private void OnCollisionEnter(Collision collision)
    {
        // 1. 부딪힌 녀석이 블록인지 확인해야 한다.
        if (collision.gameObject.name.Contains("Block"))
        {
            // PlayerAgent 에 있는 OnBlockHit 함수를 호출하고 싶다.
            player.OnBlockHit(collision.gameObject);
            // 2. 블록을 비활성화 하고 싶다.
            //collision.gameObject.SetActive(false);
        }
    }
}
