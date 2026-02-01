using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] GameObject[] m_enemys;
    // 実際の敵たちの状態
    private bool[] m_actuallyEnemyMove = null;
    // 敵が把握している敵たちの状態
    private bool[] m_graspEnemyMove = null;

    // Start is called before the first frame update
    void Start()
    {
        // 初期化
        m_graspEnemyMove = new bool[m_enemys.Length];
        m_actuallyEnemyMove = new bool[m_enemys.Length];

        int count = 0;
        foreach(GameObject enemy in m_enemys)
        {
            enemy.GetComponent<EnemyMove>().SetId(count);
            count++;
        }

        // 最初は全員動ける状態
        for (int i = 0; i <= m_enemys.Length - 1; i++)
        {
            m_actuallyEnemyMove[i] = true;
            m_graspEnemyMove[i] = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        int enemyNum = m_enemys.Length;

        // 現在の敵たちの状態を取得　敵たちが把握している敵の状態とは違う
        for (int i = 0; i > enemyNum - 1; i++)
        {
            m_actuallyEnemyMove[i] = m_enemys[i].GetComponent<EnemyMove>().GetMoveFlag();
        }

    }

    // 敵が倒れていることに気づいた処理
    // 第一引数:倒れている敵のid
    public void DiscoveryDownEnemy(int discoveryId)
    {
        //for (int i = 0; i > m_graspEnemyState.Length - 1; i++)
        //{
        //    m_actuallyEnemyState[i] = true;
        //}

        m_graspEnemyMove[discoveryId] = false;
    }

    // 敵たちが把握している敵の状態の更新
    // 第一引数:更新する敵のid 第二引数：現在の状態(falseならダウン、trueなら行動可能)
    public void UpdateGraspEnemyMove(int discoveryId,bool state)
    {
        m_graspEnemyMove[discoveryId] = state;
    }

    // 実際の敵の状態の更新
    // 第一引数:更新する敵のid 第二引数：現在の状態(falseならダウン、trueなら行動可能)
    public void UpdateActuallyEnemyMove(int discoveryId, bool state)
    {
        //// stateがfalseになるのは1人しかないのでいったんリセット
        //if(!state)
        //{
        //    for (int i = 0; i < m_graspEnemyMove.Length - 1; i++)
        //    {
        //        //m_enemys[i].GetComponent<EnemyMove>().SetMoveFlag(true);
        //    }
        //}

        Debug.Log("選択しているid:" + discoveryId);
        Debug.Log("敵の状態の配列の要素最大数:" + (m_graspEnemyMove.Length - 1));
        m_graspEnemyMove[discoveryId] = state;
        //m_enemys[discoveryId].GetComponent<EnemyMove>().SetMoveFlag(state);
    }

    public bool[] GetGraspEnemyMove()
    {
        return m_graspEnemyMove;
    }

}
