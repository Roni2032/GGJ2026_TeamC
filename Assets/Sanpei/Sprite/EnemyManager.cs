using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] GameObject[] m_enemys;
    // ÀÛ‚Ì“G‚½‚¿‚Ìó‘Ô
    private bool[] m_actuallyEnemyMove;
    // “G‚ª”cˆ¬‚µ‚Ä‚¢‚é“G‚½‚¿‚Ìó‘Ô
    private bool[] m_graspEnemyMove;

    // Start is called before the first frame update
    void Start()
    {
        int count = 0;
        foreach(GameObject enemy in m_enemys)
        {
            enemy.GetComponent<EnemyMove>().SetId(count);
            count++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        int enemyNum = m_enemys.Length;

        // Œ»İ‚Ì“G‚½‚¿‚Ìó‘Ô‚ğæ“¾@“G‚½‚¿‚ª”cˆ¬‚µ‚Ä‚¢‚é“G‚Ìó‘Ô‚Æ‚Íˆá‚¤
        for (int i = 0; i > enemyNum - 1; i++)
        {
            m_actuallyEnemyMove[i] = m_enemys[i].GetComponent<EnemyMove>().GetMoveFlag();
        }

    }

    // “G‚ª“|‚ê‚Ä‚¢‚é‚±‚Æ‚É‹C‚Ã‚¢‚½ˆ—
    // ‘æˆêˆø”:“|‚ê‚Ä‚¢‚é“G‚Ìid
    public void DiscoveryDownEnemy(int discoveryId)
    {
        //for (int i = 0; i > m_graspEnemyState.Length - 1; i++)
        //{
        //    m_actuallyEnemyState[i] = true;
        //}

        m_graspEnemyMove[discoveryId] = false;
    }

    // “G‚½‚¿‚ª”cˆ¬‚µ‚Ä‚¢‚é“G‚Ìó‘Ô‚ÌXV
    // ‘æˆêˆø”:XV‚·‚é“G‚Ìid ‘æ“ñˆø”FŒ»İ‚Ìó‘Ô(false‚È‚çƒ_ƒEƒ“Atrue‚È‚çs“®‰Â”\)
    public void UpdateGraspEnemyMove(int discoveryId,bool state)
    {
        m_graspEnemyMove[discoveryId] = state;
    }

    // ÀÛ‚Ì“G‚Ìó‘Ô‚ÌXV
    // ‘æˆêˆø”:XV‚·‚é“G‚Ìid ‘æ“ñˆø”FŒ»İ‚Ìó‘Ô(false‚È‚çƒ_ƒEƒ“Atrue‚È‚çs“®‰Â”\)
    public void UpdateActuallyEnemyMove(int discoveryId, bool state)
    {
        m_graspEnemyMove[discoveryId] = state;
    }

    public bool[] GetGraspEnemyMove()
    {
        return m_graspEnemyMove;
    }

}
