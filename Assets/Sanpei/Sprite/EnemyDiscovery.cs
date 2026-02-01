using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 敵の発見処理
public class EnemyDiscovery : MonoBehaviour
{
    // ゲームオーバフラグ
    private bool m_gameOverFlag = false;
    // 発見されるまでの許容値
    private float m_countTimeOfDiscovery = 0.0f;
    [SerializeField] float m_timeOfDiscovery = 0.0f;
    // 親オブジェクト
    [SerializeField] GameObject m_parentObj;
    // 敵マネージャ
    private GameObject m_enemyManager;
    // プレイヤーを見ているかのフラグ
    bool m_seePlayerFlag = false;

    // Start is called before the first frame update
    void Start()
    {
        // 敵マネージャ取得
        m_enemyManager = GameObject.Find("EnemyManager");
    }

    // Update is called once per frame
    void Update()
    {
        float delta = Time.deltaTime;

        if(m_seePlayerFlag)
        {
            m_countTimeOfDiscovery += delta;
            Debug.Log("発見中!：" + m_countTimeOfDiscovery);
        }

        if(m_timeOfDiscovery <= m_countTimeOfDiscovery)
        {
            m_seePlayerFlag = false;
            m_gameOverFlag = true;
        }

        // ゲームオーバフラグがオンならゲームマネージャーにゲームオーバになったことを伝える
        if(m_gameOverFlag)
        {
            // ゲームオーバーに移行
            GameManager.Instance.GameFailed();
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
            // プレイヤーが変装しているidを取得
            int currentDisguiseId = other.GetComponent<PlayerCtrl>().CurrentDisguiseID;
            // プレイヤーが変装しているidを取得
            string currentDisguiseTag = other.GetComponent<PlayerCtrl>().CurrentDisguiseTag;

            // 自分のタグと同じか確認する
            if(currentDisguiseTag == tag)
            {
                // 倒れているのを把握できているか確認する
                bool[] graspEnemyState = m_enemyManager.GetComponent<EnemyManager>().GetGraspEnemyMove();

                if (graspEnemyState[currentDisguiseId] == false)
                {
                    Debug.Log("変装した怪盗を見つけた！");

                    // プレイヤーを見たフラグオン
                    m_seePlayerFlag = true;

                }

            }
            // ゲームオーバーに移行
            //GameManager.Instance.GameFailed();
            
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

    private void OnTriggerExit(Collider other)
    {
        // 怪盗を見つけた場合ゲームオーバ
        if (other.tag == "Player")
        {
            Debug.Log("怪盗を見失った！");

            // プレイヤーを見たフラグオフ
            m_seePlayerFlag = false;

            // ゲームオーバーに移行
            //GameManager.Instance.GameFailed();

        }
    }
}
