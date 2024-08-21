using UnityEngine;
using TMPro;

/// <summary>
/// Mob（動くオブジェクト、MovingObjectの略）の状態管理スクリプト
/// </summary>

[RequireComponent(typeof(MobStatus_parameter))]
[RequireComponent(typeof(DamageUI))] //ダメージ表示に必要
public abstract class MobStatus : MonoBehaviour, ICalcDamage
{
    /// <summary>
    /// 状態の定義
    /// </summary>
    protected enum StateEnum
    {
        Normal, // 通常
        Attack, // 攻撃中
        Die,    // 死亡
        Knocked,// ノック状態
        Stunned, // スタン
        Counter,// カウンター攻撃中(無敵)
        Excavate// 発掘中
    }

    /// <summary>
    /// 移動可能かどうか
    /// </summary>
    public bool IsMovable => StateEnum.Normal == _state;

    /// <summary>
    /// 攻撃可能かどうか
    /// </summary>
    public bool IsAttackable => StateEnum.Normal == _state;

    //以下変数定義
    [SerializeField] protected MobStatus_parameter _ms_p;
    //TODO: キャラステータスの修飾子 public or protected
    public StatusManeger _status;               //status本体
    protected Animator _animator;
    protected StateEnum _state = StateEnum.Normal; //Mob状態
    protected bool immune_flag = false;            //無敵判定
    //private float _life;                         // 現在のライフ値（Debug用）



    protected virtual void Start()
    {
        _status = new StatusManeger(_ms_p);
        _animator = GetComponentInChildren<Animator>();
    }


    //interface定義の関数

    public virtual DamageObject CalcDamage(MobStatus_parameter m_p)
    {
        DamageObject d_object = new DamageObject(); //最終的に返す
        return d_object;
    }


    //　DamageUIプレハブ
    [SerializeField] public DamageUI damageUI;
    public virtual void TakeDamage(DamageObject d_object)
    {

        if (_state == StateEnum.Die) return; //死亡していたら終了
        if (immune_flag) return;              //無敵ならダメージ計算はなし

        int damage_sum = 0; //最終的なダメージ値

        int damage = d_object.Damage - _status.DEF * (1 - d_object.BreakDefense); //物理ダメージ計算,基本物理d-物理防御力*(1-貫通率)
        if (damage < 0)
        {
            damage = 0;
        }
        else if (_status.SHIELD > 0)
        {
            if (_status.SHIELD < damage) //ダメージがシールド値より大ならダメージを軽減しシールドを消失
            {
                damage -= _status.SHIELD;
                _status.SHIELD = 0;
            }
            else //ダメージがシールド値より小ならシールドを消費しダメージを消失
            {
                _status.SHIELD -= damage;
                damage = 0;
            }
        }


        int m_damage = d_object.MagicDamage - _status.MDEF * (1 - d_object.BreakMagicDefense);　//魔法ダメージ計算,基本魔法d-魔法防御力*(1-貫通率)
        if (m_damage < 0)
        {
            m_damage = 0;
        }
        else if (_status.MSHIELD > 0)
        {
            if (_status.MSHIELD < m_damage) //ダメージがシールド値より大ならダメージを軽減しシールドを消失
            {
                m_damage -= _status.MSHIELD;
                _status.MSHIELD = 0;
            }
            else //ダメージがシールド値より小ならシールドを消費しダメージを消失
            {
                _status.MSHIELD -= damage;
                m_damage = 0;
            }
        }

        damage_sum = damage + m_damage + d_object.TrueDamage; //最終的なダメージは処理後物理d・魔法d + 確定d

        new_damageUI(damage_sum);

        _status.HP -= damage_sum;

        if (_status.HP > 0) return;
        //以下死亡
        _state = StateEnum.Die;
        _animator.SetTrigger("Die");

        OnDie();
    }


    /// <summary>
    ///　DamageUIをインスタンス化。登場位置は接触したキャラの位置(仮)
    /// memo: 主に生成位置をオーバーライドする?
    /// </summary>
    /// <param name="damage"></param>
    public virtual void new_damageUI(int damage)
    {
        DamageUI obj = Instantiate<DamageUI>(damageUI, this.transform.position - Camera.main.transform.forward * 0.2f, Quaternion.identity);
        obj.Initialize(damage.ToString());//表示ダメージ量を設定

    }

    public virtual void OnDie()
    {
    }

    //interfaceの関数　終了


    /// <summary>
    /// 可能であれば攻撃中の状態に遷移します。
    /// </summary>
    public void GoToAttackStateIfPossible()
    {
        if (!IsAttackable) return;

        _state = StateEnum.Attack;
        _animator.SetTrigger("Attack");

    }

    /// <summary>
    /// 可能であればNormalの状態に遷移します。+無敵判定を解除
    /// </summary>
    public void GoToNormalStateIfPossible()
    {
        if (_state == StateEnum.Die) return;

        immune_flag = false;
        _state = StateEnum.Normal;

    }
}



