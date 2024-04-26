using System.Collections;
using UnityEngine;

/// <summary>
/// 攻撃制御クラス
/// </summary>
[RequireComponent(typeof(MobStatus))]
public class MobAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown = 5f; // 攻撃後のクールダウン（秒）
    [SerializeField] private Collider attackCollider;
    protected MobStatus _status;

    protected virtual void Start()
    {
        _status = GetComponent<MobStatus>();
    }

    /// <summary>
    /// 攻撃可能な状態であれば攻撃を行います。
    /// </summary>
    public void AttackIfPossible()
    {
        if (!_status.IsAttackable) return; 

        _status.GoToAttackStateIfPossible();
    }

    /// <summary>
    /// 攻撃対象が攻撃範囲に入った時に呼ばれます。
    /// </summary>
    /// <param name="collider"></param>
    public void OnAttackRangeEnter(Collider collider)
    {
        AttackIfPossible();
    }

    /// <summary>
    /// 攻撃の開始時に呼ばれます。
    /// </summary>
    public virtual void OnAttackStart()
    {
        //Debug.Log("Attack");
        attackCollider.enabled = true;
    }
    
    /// <summary>
    /// attackColliderが攻撃対象にHitした時に呼ばれます。
    /// </summary>
    /// <param name="collider"></param>
    public virtual void OnHitAttack(Collider collider)
    {
        var targetMob = collider.GetComponent<MobStatus>();
        if (null == targetMob) return;
        
        // 対象にダメージを与える
        targetMob.TakeDamage(new DamageObject(300,0,0,0,0));
    }
    
    /// <summary>
    /// 攻撃の終了時に呼ばれます。
    /// </summary>
    public virtual void OnAttackFinished()
    {
        //Debug.Log("Attack Finished");
        attackCollider.enabled = false;
        StartCoroutine(CooldownCoroutine());
    }

    private IEnumerator CooldownCoroutine()
    {
        yield return new WaitForSeconds(attackCooldown);
        _status.GoToNormalStateIfPossible();        
    }
}