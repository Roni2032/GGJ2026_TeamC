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
        bool moveFlag = m_parentObj.GetComponent<EnemyMove>().GetMoveFlag();

        if (!moveFlag) return;

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

        // 見失ったときにカウントする時間を減らしていく
        if (!m_seePlayerFlag && m_countTimeOfDiscovery > 0.0f)
        {
            m_countTimeOfDiscovery -= delta;
        }
        if(m_countTimeOfDiscovery < 0.0f)
        {
            m_countTimeOfDiscovery = 0.0f;
        }

        // ゲームオーバフラグがオンならゲームマネージャーにゲームオーバになったことを伝える
        if (m_gameOverFlag)
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
            // プレイヤーが変装しているか確認
            bool DisguiseFlag = other.GetComponent<PlayerCtrl>().IsDisguise;
            Debug.Log("変装しているか:" + DisguiseFlag);

            // プレイヤーが変装しているタグを取得
            //string currentDisguiseTag = other.GetComponent<PlayerCtrl>().CurrentDisguiseTag;


            // 変装している時
            if(DisguiseFlag)
            {
                // プレイヤーが変装しているidを取得
                int currentDisguiseId = other.GetComponent<PlayerCtrl>().CurrentDisguiseID;

                // プレイヤーが変装している敵がパトロールしていたエリア取得
                var disguisePatorloArea = other.GetComponent<PlayerCtrl>().CurrentDisguisePatorloArea;
                // 現在プレイヤーがいるエリア取得
                var playerArea = other.GetComponent<PlayerCtrl>().CurrentArea;

                // 現在変装している警備員のパトロールエリアとプレイヤーがいる位置が同じ場合、その警備員が倒れているのが分かっているか確認
                if (disguisePatorloArea == playerArea)
                {
                    // 倒れているのを把握できているか確認する
                    bool[] graspEnemyState = m_enemyManager.GetComponent<EnemyManager>().GetGraspEnemyMove();

                    // 倒れているのが分かっている
                    if (graspEnemyState[currentDisguiseId] == false)
                    {
                        Debug.Log("変装した怪盗を見つけた！");

                        // プレイヤーを見たフラグオン
                        m_seePlayerFlag = true;

                    }
                }
                // 変装している警備員のパトロールエリア出なかった時怪盗なのがばれる
                else if (disguisePatorloArea != playerArea)
                {
                    Debug.Log("お前何故担当から外れている!");
                    m_seePlayerFlag = true;
                }
            }
            // 変装していない時
            else if(!DisguiseFlag)
            {
                m_seePlayerFlag = true;
            }

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
                Debug.Log("警備員が倒れているのを発見した");
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
