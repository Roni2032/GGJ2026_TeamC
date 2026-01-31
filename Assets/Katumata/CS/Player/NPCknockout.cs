using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 警備員NOCをノックアウトさせる
/// </summary>
public class NPCknockout : MonoBehaviour
{
    [SerializeField] float radius = 0.0f;   // PL側で設定可能にするかは未定
    [SerializeField] LayerMask mask = 0;    // PLと地形は対象外とする

    /// <summary>
    /// Player側のinputsystemに割り当てる
    /// </summary>
    public void Knockout()
    {
        Collider[] hits = Physics.OverlapSphere(
            transform.position,
            radius,
            mask,
            QueryTriggerInteraction.Ignore      // Triggerも拾うなら Collide
        );

        foreach (var col in hits)
        {
            if (TryGetComponent(out EnemyMove enemy))
            {
                // ここで警備員を気絶させる処理を呼び出す
                // enemy.なんたら();
                Debug.Log("警備員を気絶させた！");
            }
        }
    }
}
