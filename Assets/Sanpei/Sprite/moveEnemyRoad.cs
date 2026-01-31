using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class moveEnemyRoad : MonoBehaviour
{
    // 道のポイント(配列)
    [SerializeField] GameObject[] m_moveEnemyPoints;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 敵が通るためのポイント配列のゲッタ
    public GameObject[] GetmoveEnemyPoints
    {
        get 
        {
            return m_moveEnemyPoints;
        }
    }
}
