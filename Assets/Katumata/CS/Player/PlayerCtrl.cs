using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(NPCKnockout))]
public class PlayerCtrl : MonoBehaviour
{
    [Tooltip("PL入力のInputSystem")]
    private PlayerControls _playerInput = null;

    [Tooltip("水平入力")]
    private float _horizontalValue = 0.0f;

    [Tooltip("垂直入力")]
    private float _verticalValue = 0.0f;

    [Tooltip("入力方向")]
    private Vector3 _inputDirection = Vector3.zero;

    [Tooltip("自身のRigidbody")]
    private Rigidbody _rigidbody = null;

    [Tooltip("自身のMeshFilter")]
    private MeshFilter _meshFilter = null;

    [Tooltip("自身のMeshRenderer")]
    private MeshRenderer _meshRenderer = null;

    [Tooltip("変装前のMesh")]
    private Mesh _originalMesh = null;

    [Tooltip("変装前のMat")]
    private Material _originalMat = null;

    [Tooltip("範囲内のNPCを気絶させる")]
    private NPCKnockout _npcKnockout = null;

    [SerializeField, Header("NPCKnockout用パラメータ")]
    private KnockoutParam KnockoutParam = new KnockoutParam();

    [SerializeField, Header("移動速度")]
    private float _moveSpeed = 0.0f;

    /// <summary>
    /// 
    /// </summary>
    private void Awake()
    {
        // 初期インスタンス作成
        _npcKnockout = new NPCKnockout(this, KnockoutParam);

        // PlayerControlsの初期設定
        InputSystemInitSetting();

        // RequireComponent
        _rigidbody = GetComponent<Rigidbody>();
        _meshFilter = GetComponent<MeshFilter>();
        _meshRenderer = GetComponent<MeshRenderer>();

        // 変装前の見た目を保持
        _originalMesh = _meshFilter.mesh;
        _originalMat = _meshRenderer.material;
    }

    private void OnDestroy()
    {
        _playerInput.Disable();     // 非有効化
        _playerInput.Dispose();     // リソース解放
    }

    /// <summary>
    /// PlayerControlsの初期設定
    /// </summary>
    private void InputSystemInitSetting()
    {
        // PlayerControlsをインスタンス化し、有効にする
        // リソース解放の為にフィールド変数として保持する
        _playerInput = new PlayerControls();
        _playerInput.Enable();

        // 割り当て
        {
            // Horizontalに割り当てる
            var horizontal = _playerInput.Player.Horizontal;
            horizontal.started += Horizontal;
            horizontal.canceled += Horizontal;

            // Verticalに割り当てる
            var vertical = _playerInput.Player.Vertical;
            vertical.started += Vertical;
            vertical.canceled += Vertical;

            // Knockoutに割り当てる
            var knockout = _playerInput.Player.Knockout;
            knockout.started += _npcKnockout.Knockout;
        }
    }

    /// <summary>
    /// 水平入力の処理
    /// </summary>
    /// <param name="context"></param>
    private void Horizontal(InputAction.CallbackContext context)
    {
        _inputDirection.x = context.ReadValue<float>();
    }

    /// <summary>
    /// 垂直入力の処理
    /// </summary>
    /// <param name="context"></param>
    private void Vertical(InputAction.CallbackContext context)
    {
        _inputDirection.z = context.ReadValue<float>();
    }

    void FixedUpdate()
    {
        // 移動（本当ならスレッドを作成し、移動入力を受け付けない時はWaitさせる方が綺麗なコードになる）
        {
            if (_inputDirection.sqrMagnitude > 1.0f)
            {
                // 正規化
                _inputDirection.Normalize();
            }

            Vector3 v = _rigidbody.velocity;
            Vector3 horiz = new Vector3(v.x, 0f, v.z);

            // 一定速度
            Vector3 targetHoriz = _inputDirection * _moveSpeed;

            // 差分だけ速度変更
            Vector3 deltaV = targetHoriz - horiz;
            _rigidbody.AddForce(deltaV, ForceMode.VelocityChange);
        }
    }

    /// <summary>
    /// 変装
    /// </summary>
    /// <param name="mesh"></param>
    /// <param name="material"></param>
    public void Disguise(Mesh mesh, Material material)
    {
        _meshFilter.mesh = mesh;
        _meshRenderer.material = material;
    }

    /// <summary>
    /// 変装を解く
    /// </summary>
    public void Undisguise()
    {
        _meshFilter.mesh = _originalMesh;
        _meshRenderer.material = _originalMat;
    }
}
