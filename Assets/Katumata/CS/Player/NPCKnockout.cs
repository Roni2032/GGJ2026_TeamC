using System;
using UnityEngine;
using UnityEngine.InputSystem;

#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using static UnityEngine.EventSystems.EventTrigger;
#endif

/// <summary>
/// NPCKnockout用パラメータ
/// </summary>
[Serializable]
public struct KnockoutParam
{
    [Header("ノックアウト範囲")]
    public float radius;

    [Header("気絶対象のLayer")]
    public LayerMask mask;     // PLと地形を対象外にする

    [Header("サウンドマネージャー")]
    public SoundManager soundManager;
}

/// <summary>
/// 範囲内のNPCを気絶させる
/// </summary>
public class NPCKnockout
{
    [Tooltip("ノックアウト範囲")]
    private float _radius = 0.0f;

    [Tooltip("気絶対象のLayer")]
    private LayerMask _mask = 0;     // PLと地形を対象外にする

    [Tooltip("気絶を実行するPL")]
    private PlayerCtrl _playerCtrl = null;

    [Tooltip("現在気絶中のNPC")]
    private EnemyMove _currentKnockoutEnemy = null;

    [Tooltip("変装前のMeshFilter")]
    private MeshFilter _originalMeshFilter = null;

    [Tooltip("変装前のMeshRenderer")]
    private MeshRenderer _originalMeshRenderer = null;

    [Tooltip("サウンドマネージャー")]
    private SoundManager soundManager;

    public NPCKnockout(PlayerCtrl playerCtrl, KnockoutParam knockoutParam)
    {
        _playerCtrl = playerCtrl;

        // パラメータ
        _radius = knockoutParam.radius;
        _mask = knockoutParam.mask;
        soundManager = knockoutParam.soundManager;

        // RequireComponent
        {
            // 変装前の見た目を保持
            _originalMeshFilter = playerCtrl.gameObject.GetComponent<MeshFilter>();
            _originalMeshRenderer = playerCtrl.gameObject.GetComponent<MeshRenderer>();
        }
    }

    /// <summary>
    /// 気絶させる。<br/>
    /// PLのInputSystemに割り当てる
    /// </summary>
    public void Knockout(InputAction.CallbackContext context)
    {
        Collider[] others = Physics.OverlapSphere(
            _playerCtrl.transform.position,
            _radius,
            _mask,
            QueryTriggerInteraction.Ignore // Triggerも拾うなら Collide
        );

        Vector3 origin = _playerCtrl.transform.position;

        EnemyMove nearestEnemy = null;
        float bestSqrDist = float.PositiveInfinity;

        // 範囲内に気絶対象があるか探索
        foreach (var col in others)
        {
            if (col.gameObject.TryGetComponent(out EnemyMove enemy))
            {
                float sqrDist = (col.transform.position - origin).sqrMagnitude;

                if (sqrDist < bestSqrDist)
                {
                    bestSqrDist = sqrDist;
                    nearestEnemy = enemy;
                }
            }
        }

        // nearest が一番近い対象（見つからなければ null）
        if (nearestEnemy != null)
        {
#if false   // DEL
            // ここで、現在気絶中のNPCを起こす
            //_currentKnockoutEnemy.なんたら();
#endif

            // 気絶中のNPCを更新
            _currentKnockoutEnemy = nearestEnemy;

            // ここで警備員の気絶処理を呼び出す
            //_currentKnockoutEnemy.なんたら();
            _currentKnockoutEnemy.SetMoveFlag(false);
            soundManager.OnStrike();

            // 気絶させた警備員に変装する
            {
                if (_currentKnockoutEnemy.gameObject.TryGetComponent(out MeshFilter meshFilter) != true)
                {
                    Debug.LogError("対象にMeshFilterがアタッチされていません！");
                }
                if (_currentKnockoutEnemy.gameObject.TryGetComponent(out MeshRenderer meshRenderer) != true)
                {
                    Debug.LogError("対象にMeshRendererがアタッチされていません！");
                }

                // 変装
                _playerCtrl.Disguise(meshFilter.mesh, meshRenderer.material, _currentKnockoutEnemy);
            }
        }
    }
}
