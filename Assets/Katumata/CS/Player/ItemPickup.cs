using System;
using UnityEngine;
using UnityEngine.InputSystem;

#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using static UnityEngine.EventSystems.EventTrigger;
#endif

/// <summary>
/// ItemPickup用パラメータ
/// </summary>
[Serializable]
public struct ItemPickupParam
{
    [Header("ピックアップ範囲")]
    public float radius;

    [Header("ピックアップ対象のLayer")]
    public LayerMask mask;     // PLと地形を対象外にする

    [Header("サウンドマネージャー")]
    public SoundManager soundManager;

    [Header("取得時のオフセット")]
    public Transform pickupOffset;
}

/// <summary>
/// アイテムを拾う
/// </summary>
public class ItemPickup
{
    [Tooltip("ピックアップ範囲")]
    private float _radius = 0.0f;

    [Tooltip("ピックアップ対象のLayer")]
    private LayerMask _mask = 0;     // PLと地形を対象外にする

    [Tooltip("ピックアップを実行するPL")]
    private PlayerCtrl _playerCtrl = null;

    [Tooltip("サウンドマネージャー")]
    private SoundManager soundManager;

    [Tooltip("所持中のアイテム")]
    private GameObject _pickupItemObj = null;

    [Tooltip("宝石を拾ったか")]
    private bool _isGemPickup = false;

    [Header("取得時のオフセット")]
    private Transform _pickupOffset = null;

    /// <summary>
    /// 宝石を拾ったか
    /// </summary>
    public bool IsGemPickup => _isGemPickup;

    public ItemPickup(PlayerCtrl playerCtrl, ItemPickupParam itemPickupParam)
    {
        _radius = itemPickupParam.radius;
        _mask = itemPickupParam.mask;
        _mask = itemPickupParam.mask;
        soundManager = itemPickupParam.soundManager;
        _pickupOffset = itemPickupParam.pickupOffset;

        _playerCtrl = playerCtrl;
    }

    /// <summary>
    /// 拾う。<br/>
    /// PLのInputSystemに割り当てる
    /// </summary>
    /// <param name="context"></param>
    public void Pickup(InputAction.CallbackContext context)
    {
        const string GEM_TAG = "Gem";

        Collider[] others = Physics.OverlapSphere(
            _playerCtrl.transform.position,
            _radius,
            _mask,
            QueryTriggerInteraction.Ignore // Triggerも拾うなら Collide
        );

        Vector3 origin = _playerCtrl.transform.position;

        GameObject nearestItem = null;
        float bestSqrDist = float.PositiveInfinity;

        // 範囲内にピックアップ対象があるか探索
        foreach (var col in others)
        {
            GameObject obj = col.gameObject;
            if (obj.tag == GEM_TAG)
            {
                float sqrDist = (col.transform.position - origin).sqrMagnitude;

                if (sqrDist < bestSqrDist)
                {
                    bestSqrDist = sqrDist;
                    nearestItem = obj;
                }
            }
        }

        // nearest が一番近い対象（見つからなければ null）
        if (nearestItem != null)
        {
            // 一番近いアイテムを拾う
            _pickupItemObj = nearestItem;
            _pickupItemObj.transform.parent = _pickupOffset.transform;
            _pickupItemObj.transform.localPosition = Vector3.zero;
        }
    }
}
