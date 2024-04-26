using System;

public interface ICalcDamage
{
    /// <summary>
    /// 攻撃者の各ステータスに基づいたダメージを計算します
    /// </summary>
    public DamageObject CalcDamage(MobStatus_parameter m_p);

    /// <summary>
    /// ダメージ処理とHit状態転移を記述します。
    /// </summary>
    public void TakeDamage(DamageObject d_object);

    /// <summary>
    /// キャラが倒れた時の処理を記述します。
    /// </summary>
    public void OnDie();
}

