using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStatus : MobStatus
{
    [SerializeField] String directionScene = "GameOverScene";

    /// <summary>
    /// カウンター可能かどうか
    /// </summary>
    public bool IsCounterable => StateEnum.Normal == _state;

    /// <summary>
    /// 発掘動作可能かどうか
    /// </summary>
    public bool IsExcavable => StateEnum.Normal == _state;

    /// <summary>
    /// 死亡時の挙動(GameOver)
    /// </summary>
    public override void OnDie()
    {
        base.OnDie();
        StartCoroutine(DeathCoroutine());
    }

    ///
    ///以下様々な攻撃に対する状態転移
    ///

    /// <summary>
    /// 可能であれば反撃態勢の状態に遷移します。
    /// 状態変化なし、animatorのtriggerのみ変更
    /// </summary>
    public void GoToC_PrepareStateIfPossible()
    {
        if (!IsAttackable) return;

        _animator.SetTrigger("C_Prepare");
    }

    /// <summary>
    /// 可能であればカウンター不発時の通常攻撃の状態に遷移します。
    /// </summary>
    public void GoToC_AttackStateIfPossible()
    {
        if (!IsAttackable) return;

        _state = StateEnum.Attack;
        _animator.SetTrigger("C_Attack");
    }


    /// <summary>
    /// 可能であればカウンター攻撃の状態に遷移します。
    /// counter_Colliderが攻撃にHitした時に呼ばれます
    /// </summary>
    public void GoToCounterStateIfPossible()
    {
        if (!IsCounterable) return;

        _state = StateEnum.Counter;
        immune_flag = true; //無敵状態にする
        _animator.SetTrigger("Counter");
    }

    /// <summary>
    /// 可能であれば発掘動作の状態に遷移します。
    /// </summary>
    public void GoToExcaveIfPossible()
    {
        if (!IsExcavable) return;

        _state = StateEnum.Excavate;
        _animator.SetTrigger("Excave");
    }

    ///
    ///状態転移群ここまで
    ///


    /// <summary>
    /// 倒された時の消滅コルーチンです。
    /// </summary>
    /// <returns></returns>
    private IEnumerator DeathCoroutine()
    {
        yield return new WaitForSeconds(3);

        // シーン遷移の際にはSceneManagerを使用する
        //SceneManager.LoadScene(direction_Scene);
        SceneManager.LoadSceneAsync(directionScene);

    }

    public override void new_damageUI(int damage)
    {
        DamageUI obj = Instantiate<DamageUI>(damageUI, this.transform.position + new Vector3(0, 3.5f, 0) - Camera.main.transform.forward * 0.2f, Quaternion.identity);
        obj.Initialize(damage.ToString());//表示ダメージ量を設定

    }
}