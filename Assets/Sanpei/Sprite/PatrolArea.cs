using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 警備範囲の管理
public class PatrolArea : MonoBehaviour
{
    [SerializeField] int m_areaId;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // 当たっていたのがPlayerならそれに対してあなたはこのエリアに居ますよと伝える
        if (other.gameObject.TryGetComponent(out PlayerCtrl playerCtrl))
        {
            playerCtrl.CurrentArea = this;
        }
        else if(other.gameObject.TryGetComponent(out EnemyMove enemyMove))
        {
            enemyMove.SetMyArea(this);
        }

    }

    // エリアidのゲッタ
    public int GetAreaId()
    {
        return m_areaId;
    }
}
