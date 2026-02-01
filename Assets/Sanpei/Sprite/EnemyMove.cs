using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    enum ENEMYSTATE
    {
        None,
        Walk,
        Rotation
    }
    // 現在のステート
    private ENEMYSTATE m_currentState = ENEMYSTATE.Rotation;

    // 自分のID
    private int m_id;

    // 敵マネージャ
    [SerializeField] GameObject m_enemyManager;

    // 敵が通るための道
    [SerializeField] GameObject m_moveEnemyRoad;
    private GameObject[] m_moveEnemyPoints;
    private GameObject m_targetObj;

    // 現在の位置
    private Vector3 m_pos;

    // 速度
    [SerializeField] float m_speed;
    // デルタタイム
    private float m_delta;

    private int m_movePhase;
    private int m_movePhaseMax;

    // フェーズの動き方のフラグ　falseなら-trueなら+
    bool m_isPhaseMoving = true;

    // 現在動いていいかのフラグ
    [SerializeField]bool m_moveFlag = true;
    private bool m_moveFlagBefore = true;

    // Start is called before the first frame update
    void Start()
    {
        // 自分の位置取得
        m_pos = transform.position;

        m_moveEnemyPoints = m_moveEnemyRoad.GetComponent<moveEnemyRoad>().GetmoveEnemyPoints;
        // 道のポイントの数を取得
        m_movePhaseMax = m_moveEnemyPoints.Length;
        // 進むための目標を初期化
        m_targetObj = m_moveEnemyPoints[m_movePhase];
    }

    // Update is called once per frame
    void Update()
    {
        if(m_moveFlag != m_moveFlagBefore)
        {
            // ダウンから復活した場合敵全体にも復活したと伝えるようにする
            if(!m_moveFlagBefore)
            {
                m_enemyManager.GetComponent<EnemyManager>().UpdateGraspEnemyState(m_id, true);
                m_enemyManager.GetComponent<EnemyManager>().UpdateActuallyEnemyState(m_id, true);
            }
            else if(m_moveFlagBefore)
            {
                // 実際の敵の状態を渡す(敵たち自体は倒れている敵を見ないと把握できない)
                m_enemyManager.GetComponent<EnemyManager>().UpdateActuallyEnemyState(m_id, true);
            }

            m_moveFlagBefore = m_moveFlag;
        }

        // moveFlagがfalseなら動いてはいけないのでreturnする
        if (!m_moveFlag) return;

        // デルタタイム取得
        m_delta = Time.deltaTime;

        // 進むための目標位置取得
        Vector3 targetPos = m_moveEnemyPoints[m_movePhase].GetComponent<Transform>().position;

        // 現在目標の方向に進む、進み終わったら次の目標に移行して進む

        // 移動ベクトル計算
        Vector3 moveVec = targetPos - m_pos;
        // 方向ベクトル取得
        Vector3 directionVec = moveVec.normalized;

        // 現在ステートによって歩くか回転して軸合わせをするか決める
        if(m_currentState == ENEMYSTATE.Walk)
        {
            // 移動処理
            m_pos = (m_pos + (directionVec * m_speed) * m_delta);
            transform.position = m_pos;
        }
        else if(m_currentState == ENEMYSTATE.Rotation)
        {
            // 回転処理
            Quaternion rot = transform.rotation;
            Quaternion targetRot = Quaternion.LookRotation(moveVec);
            Debug.Log(targetRot);
            rot = Quaternion.Lerp(rot, targetRot, 0.25f);
            transform.rotation = rot;

            float disAngle = Quaternion.Angle(rot, targetRot);
            // ある程度目標地点まで回転できたとみなせるなら歩きステートに変更する
            if(disAngle <= 1.0f)
            {
                // ステート変更
                transform.rotation = targetRot;
                m_currentState = ENEMYSTATE.Walk;
            }
        }

        // 移動後目標位置までついたとみなしたら次の目標に移行する
        moveVec = targetPos - m_pos;

        //Debug.Log("現在の目標までの位置：" + moveVec.magnitude);
        // 最後の目標位置まで到着した場合は進んだ道を逆走する
        if (moveVec.magnitude <= 0.3f)
        {
            // ステート変更
            m_currentState = ENEMYSTATE.Rotation;

            if (m_movePhase == m_movePhaseMax-1)
            {
                m_isPhaseMoving = false;
            }
            else if (m_movePhase == 0)
            {
                m_isPhaseMoving = true;
            }

            if (m_isPhaseMoving)
            {
                m_movePhase++;
            }
            else if (!m_isPhaseMoving)
            {
                m_movePhase--;
            }
        }
    }

    // Posの移動処理
    private void movePos()
    {
        // デルタタイム取得
        m_delta = Time.deltaTime;

        // 進むための目標位置取得
        Vector3 targetPos = m_moveEnemyPoints[m_movePhase].GetComponent<Transform>().position;

        // 現在目標の方向に進む、進み終わったら次の目標に移行して進む

        // 移動ベクトル計算
        Vector3 moveVec = targetPos - m_pos;
        // 方向ベクトル取得
        Vector3 directionVec = moveVec.normalized;

        // 移動処理
        m_pos = (m_pos + (directionVec * m_speed) * m_delta);
        transform.position = m_pos;
    }

    // 動いていいかのゲッタ
    public bool GetMoveFlag()
    {
        return m_moveFlag;
    }
    // 動いていいかのセッタ
    public void SetMoveFlag(bool moveFlag)
    {
        m_moveFlag = moveFlag;
    }

    // idのゲッタ
    public int GetId()
    {
        return m_id;
    }
    // idのセッタ
    public void SetId(int id)
    {
        m_id = id;
    }
}
