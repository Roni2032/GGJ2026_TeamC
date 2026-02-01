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
        // 怪盗を見つけた場合ゲームオーバ
        if(other.tag == "Player")
        {
            Debug.Log("怪盗を見つけた！");
            m_gameOverFlag = true;
            // ゲームオーバーに移行
            GameManager.Instance.GameFailed();
            
        }
        //Debug.Log("ぶつかった");

        // 敵(警備員)を見つけた処理
        if(other.tag == "Enemy")
        {
            // 見つけた敵の状態を確認する
            EnemyMove enemyMove = other.GetComponent<EnemyMove>();
            bool discoveryEnemyMoveFlag = enemyMove.GetMoveFlag();

        　　// 見つけた敵が倒れていることに気づいたとき敵全体に伝える
            if (!discoveryEnemyMoveFlag)
            {
                int discoveryEnemyId = enemyMove.GetId();
                GameObject.Find("EnemyManager").GetComponent<EnemyManager>().DiscoveryDownEnemy(discoveryEnemyId);
            }
        }
    }
}
