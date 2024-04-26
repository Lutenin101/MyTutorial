using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerStatus))]
[RequireComponent(typeof(PlayerAttack))]
public class PlayerController1 : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float moveSpeed = 3; // 移動速度
    [SerializeField] private float jumpPower = 3; // ジャンプ力
    private CharacterController _characterController; // CharacterControllerのキャッシュ
    private Transform _transform; // Transformのキャッシュ
    private Vector3 _moveVelocity; // キャラの移動速度情報
    private PlayerStatus _status;
    private PlayerAttack _attack;
    Transform cameraTrn;
    Vector2 moveInput; // 移動入力
    bool jumpInput;

    private void Start()
    {
        cameraTrn = Camera.main.transform;
        _characterController = GetComponent<CharacterController>(); // 毎フレームアクセスするので、負荷を下げるためにキャッシュしておく
        _transform = transform; // Transformもキャッシュすると少しだけ負荷が下がる
        _status = GetComponent<PlayerStatus>();
        _attack = GetComponent<PlayerAttack>();
    }

    private void Update()
    {

        //TODO <<入力群をInputSystemに変更する>>

        Debug.Log(_characterController.isGrounded ? "地上にいます" : "空中です");

        if (Input.GetButtonDown("Fire1"))
        {
            // Fire1ボタン（デフォルトだとマウス左クリック）で攻撃
            _attack.AttackIfPossible();
        }

        if (Input.GetButtonDown("Fire2"))
        {
            // Fire2（デフォルトだとマウス右クリック）で攻撃
            // カウンター成功判定はcollider内でされ、成功すれば攻撃がカウンターへと派生
            _attack.C_PrepareIfPossible();
        }

        if (Input.GetKey(KeyCode.Return))
        {
            if (!_status.IsExcavable) return;

            _status.GoToExcaveIfPossible();
        }

        if (_status.IsMovable) // 移動可能な状態であれば、ユーザー入力を移動に反映する
        {
            var dir = transform.position - new Vector3(cameraTrn.position.x, transform.position.y, cameraTrn.position.z);
            dir = dir.normalized;

            //moveDirection = orientationTrn.forward * moveInput.y + orientationTrn.right * moveInput.x;
            _moveVelocity = (dir * Input.GetAxis("Vertical") +
                Quaternion.Euler(0, 90, 0) * dir * Input.GetAxis("Horizontal")) * moveSpeed;

            // 移動方向に向く
            _transform.LookAt(_transform.position + _moveVelocity);
        }
        else
        {
            _moveVelocity.x = 0;
            _moveVelocity.z = 0;
        }

        if (_characterController.isGrounded)
        {
            if (Input.GetButtonDown("Jump"))
            {
                // ジャンプ処理
                Debug.Log("ジャンプ！");
                _moveVelocity.y = jumpPower; // ジャンプの際は上方向に移動させる
            }
        }

        // 重力による加速
        _moveVelocity.y += Physics.gravity.y;

        Debug.Log("Player velocity: " + _moveVelocity);

        // オブジェクトを動かす
        _characterController.Move(_moveVelocity * Time.deltaTime);

        // 移動スピードをanimatorに反映
        animator.SetFloat("MoveSpeed", new Vector3(_moveVelocity.x, 0, _moveVelocity.z).magnitude);
    }
}
