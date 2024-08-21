using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerStatus))]
public class PlayerAttack : MobAttack
{
    [SerializeField] private int Counter_WaitFrame = 5; //カウンター受付frame数
    [SerializeField] private float counterCooldown = 0.5f; //カウンターのクールダウン
    [SerializeField] private float excaveCooldown = 1f; // 発掘動作後のクールダウン（秒）
    [SerializeField] private Collider counterCollider; //カウンター可否判定用
    [SerializeField] private Collider treasureDetectCollider; //カウンター可否判定用
    private PlayerStatus _p_status;


    protected override void Start()
    {
        _p_status = GetComponent<PlayerStatus>();
        _status = _p_status; //baseクラスの関数を呼ぶのに必要
    }

    public void C_PrepareIfPossible() //カウンター準備態勢に移行
    {
        if (!_status.IsAttackable) return;

        StartCoroutine(OnCounter_Cotoutine());
        _p_status.GoToC_PrepareStateIfPossible();
    }

    /// <summary>
    /// counter_Colliderをonにし、指定frame数後off
    /// offになった際にカウンター不発であれば通常攻撃に移行する
    /// </summary>
    private IEnumerator OnCounter_Cotoutine()
    {

        counterCollider.enabled = true;
        //Debug.Log("CounterCollider ON");

        // フィールドで指定したフレーム数だけ待ちます。
        for (int i = 0; i < Counter_WaitFrame; i++)
        {
            yield return null;
        }

        counterCollider.enabled = false;

        if (_status.IsAttackable) //カウンター不発時の挙動
        {
            _p_status.GoToC_AttackStateIfPossible();
        }
        //Debug.Log("CounterCollider OFF");
    }


    /// <summary>
    /// カウンター不発時の通常攻撃の開始時に呼ばれます。
    /// 終了時はOnAttackfinished()
    /// </summary>
    public virtual void OnC_AttackStart()
    {
        if (!_status.IsAttackable) return;
        //カウンター成功時はCounter状態のため不発時のみ実行

        Debug.Log("C_Attack");
        base.OnAttackStart(); //baseクラスの通常攻撃
    }





    /// <summary>
    /// counter_Colliderが攻撃にHitし、Counterに状態転移した際に呼ばれます
    /// </summary>
    public virtual void OnCounterStart()
    {
        Debug.Log("Counter Success");
        counterCollider.enabled = false; //Colliderを無効化、重複防止

    }

    /// <summary>
    /// カウンターの終了時に呼ばれます。
    /// </summary>
    public virtual void OnCounterFinished()
    {
        StartCoroutine(Counter_CooldownCoroutine());
    }

    private IEnumerator Counter_CooldownCoroutine()
    {
        yield return new WaitForSeconds(counterCooldown);
        _p_status.GoToNormalStateIfPossible();
    }


    /// <summary>
    /// 発掘動作の開始時に呼ばれます。
    /// </summary>
    public virtual void OnExcaveStart()
    {
        //Debug.Log("Excaved");
        treasureDetectCollider.enabled = true;
    }

    /// <summary>
    /// treasureDetectColliderが対象(treasure)にHitした時に呼ばれます。
    /// </summary>
    /// <param name="collider"></param>
    public virtual void OnFoundTreasure(Collider collider)
    {
        var targetObj = collider.GetComponent<Treasure>();
        if (null == targetObj)
        {
            Debug.Log("Not Found");
            return;
        }
        // 対象の発見を伝達
        targetObj.Detected();
    }

    /// <summary>
    /// 発掘動作の終了時に呼ばれます。
    /// </summary>
    public virtual void OnExcaveFinished()
    {
        //Debug.Log("Excave Finished");
        treasureDetectCollider.enabled = false;
        StartCoroutine(excaveCooldownCoroutine());
    }

    private IEnumerator excaveCooldownCoroutine()
    {
        yield return new WaitForSeconds(excaveCooldown);
        _status.GoToNormalStateIfPossible();
    }
}
