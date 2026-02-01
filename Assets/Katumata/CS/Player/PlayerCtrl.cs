#define SkinnedMeshRenderer

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

[RequireComponent(typeof(Rigidbody))]

#if SkinnedMeshRenderer

#else
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
#endif
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

    [Tooltip("アイテムを拾う")]
    private ItemPickup _itemPickup = null;

    [Tooltip("使用中のカメラ")]
    private Camera _cam = null;

    [Tooltip("現在変装中の警備員")]
    private EnemyMove _currentDisguiseEnemy = null;

    [Tooltip("現在変装中の警備員のTag")]
    private string _currentDisguiseTag = string.Empty;

    [Tooltip("現在変装中の警備員の警備エリア")]
    private PatrolArea _currentDisguisePatorloArea = null;

    [Tooltip("変装中か")]
    private bool _isDisguise = false;

    [Tooltip("現在いる範囲")]
    private PatrolArea _currentPatorlArea = null;

    [SerializeField, Header("NPCKnockout用パラメータ")]
    private KnockoutParam _knockoutParam = new KnockoutParam();

    [SerializeField, Header("ItemPickup用パラメータ")]
    private ItemPickupParam _itemPickupParam = new ItemPickupParam();

    [SerializeField, Header("移動速度")]
    private float _moveSpeed = 0.0f;

    [SerializeField, Header("回転速度")] 
    private float _turnSpeed = 0.0f;

    [SerializeField, Header("サウンドマネージャー")]
    private SoundManager _soundManager;

#if SkinnedMeshRenderer
    [SerializeField, Header("自身のSkinnedMeshRenderer")]
    private SkinnedMeshRenderer _skinnedMeshRenderer;
#endif

    /// <summary>
    /// 現在変装中の警備員のID
    /// </summary>
    public int CurrentDisguiseID => _currentDisguiseEnemy.GetId();

    /// <summary>
    /// 現在変装中の警備員のTag
    /// </summary>
    public string CurrentDisguiseTag => _currentDisguiseTag;

    /// <summary>
    /// 現在変装中の警備員の警備エリア
    /// </summary>
    public PatrolArea CurrentDisguisePatorloArea => _currentDisguisePatorloArea;

    /// <summary>
    /// 変装中か
    /// </summary>
    public bool IsDisguise => _isDisguise;

    /// <summary>
    /// 宝石を拾ったか
    /// </summary>
    public bool IsGemPickup => _itemPickup.IsGemPickup;

    /// <summary>
    /// 現在いる範囲
    /// </summary>
    public PatrolArea CurrentArea
    {
        get => _currentPatorlArea;
        set
        {
            if (value != _currentPatorlArea)
            {
                _currentPatorlArea = value;

                // 値変化時の処理
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void Awake()
    {
        // 初期インスタンス作成
        _npcKnockout = new NPCKnockout(this, _knockoutParam);
        _itemPickup = new ItemPickup(this, _itemPickupParam);

        // PlayerControlsの初期設定
        InputSystemInitSetting();

        // RequireComponent
        _rigidbody = GetComponent<Rigidbody>();
        _meshFilter = GetComponent<MeshFilter>();
        _meshRenderer = GetComponent<MeshRenderer>();

#if SkinnedMeshRenderer
        // 変装前の見た目を保持
        _originalMesh = _skinnedMeshRenderer.sharedMesh;
        _originalMat = _skinnedMeshRenderer.material;

#else
        // 変装前の見た目を保持
        _originalMesh = _meshFilter.mesh;
        _originalMat = _meshRenderer.material;
#endif

        // メインカメラ取得
        _cam = Camera.main;
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

            // ItemPickupに割り当てる
            var itemPickup = _playerInput.Player.ItemPickup;
            itemPickup.started += _itemPickup.Pickup;
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
            Vector3 moveDir = _inputDirection;

            // カメラがnullでなければ、カメラの角度を考慮する
            if (_cam != null)
            {
                // カメラ基準でワールド移動方向へ変換
                Vector3 camForward = _cam.transform.forward;
                camForward.y = 0f;
                camForward.Normalize();

                Vector3 camRight = _cam.transform.right;
                camRight.y = 0f;
                camRight.Normalize();

                // カメラ基準の移動方向を計算
                moveDir = camRight * _inputDirection.x + camForward * _inputDirection.z;
            }

            if (moveDir.sqrMagnitude > 1.0f)
            {
                // 正規化
                moveDir.Normalize();
            }

            Vector3 v = _rigidbody.velocity;
            Vector3 horiz = new Vector3(v.x, 0f, v.z);

            // 一定速度
            Vector3 targetHoriz = moveDir * _moveSpeed;

            // 差分だけ速度変更
            Vector3 deltaV = targetHoriz - horiz;
            _rigidbody.AddForce(deltaV, ForceMode.VelocityChange);



            // 回転
            {
                // 進行方向を向く
                Quaternion targetRot = Quaternion.LookRotation(moveDir, Vector3.up);

                // なめらかに回す
                Quaternion newRot = Quaternion.RotateTowards(
                    _rigidbody.rotation,
                    targetRot,
                    _turnSpeed * Time.fixedDeltaTime
                );

                _rigidbody.MoveRotation(newRot);
            }
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

        _isDisguise = true;

        _soundManager.OnDisguise();
    }

    /// <summary>
    /// 変装
    /// </summary>
    /// <param name="mesh"></param>
    /// <param name="material"></param>
    public void Disguise(Mesh mesh, Material material, EnemyMove enemy)
    {
        _meshFilter.mesh = mesh;
        _meshRenderer.material = material;

        // 現在変装中の警備員とTagを取得
        _currentDisguiseEnemy = enemy;
        _currentDisguiseTag = enemy.gameObject.tag;
        _currentDisguisePatorloArea = enemy.GetMyArea();

        _isDisguise = true;
        _soundManager.OnDisguise();
    }

    /// <summary>
    /// 変装を解く
    /// </summary>
    public void Undisguise()
    {
#if SkinnedMeshRenderer
        // 変装前の見た目を保持
        _skinnedMeshRenderer.sharedMesh = _originalMesh;
        _skinnedMeshRenderer.material = _originalMat;
#else
        _meshFilter.mesh = _originalMesh;
        _meshRenderer.material = _originalMat;
#endif

        _isDisguise = false;
    }
}
