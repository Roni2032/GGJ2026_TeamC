using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 敵の発見処理
public class EnemyDiscovery : MonoBehaviour
{
    // ゲームオーバフラグ
    bool m_gameOverFlag = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // ゲームオーバフラグがオンならゲームマネージャーにゲームオーバになったことを伝える
        if(m_gameOverFlag)
        {

        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    Debug.Log("ぶつかった");
    //}

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("怪盗を見つけた！");
            m_gameOverFlag = true;
            
        }
        //Debug.Log("ぶつかった");
    }
}
