using System;
using UnityEngine;

//namespace AssemblyCSharp.Assets.MyGame.Scripts
/// <summary>
/// ダメージに関するオブジェクト情報を格納
/// </summary>
public class DamageObject
{
    [SerializeField] private int damage = 100; //物理ダメージ
    [SerializeField] private int magicDamage = 100;//魔法ダメージ
    [SerializeField] private int trueDamage = 0; //確定ダメージ
    [SerializeField] private int breakDefense = 0; //物理貫通率(%)
    [SerializeField] private int breakMagicDefense = 0;//魔法貫通率(%)

    public DamageObject() { } //初期値のままで生成する

    public DamageObject(int d, int m_d, int t_d, int b_d, int b_md) //パラメーター全てを引数にとる
    {
        this.damage = d;
        this.magicDamage = m_d;
        this.trueDamage = t_d;
        this.breakDefense = b_d;
        this.breakMagicDefense = b_md;
    }

    /// <summary>
    /// ダメージ一律加算
    /// </summary>
    public DamageObject AddDamage(int add)
    {
        this.damage += add;
        this.magicDamage += add;
        this.trueDamage += add;

        return this;
    }

    /// <summary>
    /// ダメージ一律 (乗or除)算(小数点切り捨て)
    /// </summary>
    public DamageObject MultiplyDamage(float n)
    {
        this.damage *= (int)n;
        this.magicDamage *= (int)n;
        this.trueDamage *= (int)n;

        return this;
    }


    //アクセッサ
    public int Damage
    {
        get { return damage; }
        set
        {
            if (value > 0) damage = value;
            else damage = 0;
        }
    }

    public int MagicDamage
    {
        get { return magicDamage; }
        set
        {
            if (value > 0) magicDamage = value;
            else magicDamage = 0;
        }
    }

    public int TrueDamage
    {
        get { return trueDamage; }
        set
        {
            if (value > 0) trueDamage = value;
            else trueDamage = 0;
        }
    }

    public int BreakDefense
    {
        get { return breakDefense; }
        set
        {
            if (value > 0) breakDefense = value;
            else if (value > 100) breakDefense = 100;
            else breakDefense = 0;
        }
    }

    public int BreakMagicDefense
    {
        get { return breakMagicDefense; }
        set
        {
            if (value > 0) breakMagicDefense = value;
            else if (value > 100) breakMagicDefense = 100;
            else breakMagicDefense = 0;
        }
    }
}

