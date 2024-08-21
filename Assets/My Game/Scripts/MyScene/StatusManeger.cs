using System.Collections;
using UnityEngine;

/// <summary>
/// Mobのステータス情報(全てのパラメータは変数)
/// MEMO: ScriptableObjectと同じ種類の変数を持つ必要があり、要審議
/// </summary>
public class StatusManeger
{
    private int MAXHP;//最大体力
    private int hp;   //現在体力
    private int atk;  //物理攻撃力
    private int matk; //魔法攻撃力
    private int def;  //物理防御力
    private int mdef; //魔法防御力
    private int ms;   //移動スピード

    private int shield;  //物理障壁
    private int mshield; //魔法障壁

    private MobStatus_parameter base_status;

    ///TODO バフ/デバフ/アイテム,スキル効果を保持するリストを作る

    /// <summary>
    /// コンストラクタ
    /// 引数に基礎Statusとなるパラメータ(ScriptableObject)を与える。
    /// </summary>
    public StatusManeger(MobStatus_parameter s)
    {
        base_status = s;
        Init_status();
    }


    //アクセッサ
    public int MAX_HP
    {
        get { return MAXHP; }
        set
        {
            if (value > 0) MAXHP = value;
            else MAXHP = 0;
        }
    }

    public int HP
    {
        get { return hp; }
        set
        {
            if (value > 0) hp = value;
            else hp = 0;
        }
    }

    public int ATK
    {
        get { return atk; }
        set
        {
            if (value > 0) atk = value;
            else atk = 0;
        }
    }

    public int MATK
    {
        get { return matk; }
        set
        {
            if (value > 0) matk = value;
            else matk = 0;
        }
    }

    public int DEF
    {
        get { return def; }
        set
        {
            if (value > 0) def = value;
            else def = 0;
        }
    }

    public int MDEF
    {
        get { return mdef; }
        set
        {
            if (value > 0) mdef = value;
            else mdef = 0;
        }
    }

    public int MS
    {
        get { return ms; }
        set
        {
            if (value > 0) ms = value;
            else ms = 0;
        }
    }

    public int SHIELD
    {
        get { return shield; }
        set
        {
            if (value > 0) shield = value;
            else shield = 0;
        }
    }

    public int MSHIELD
    {
        get { return mshield; }
        set
        {
            if (value > 0) mshield = value;
            else mshield = 0;
        }
    }

    /// <summary>
    /// 全ステータスを初期化する
    /// MEMO: ステータス数変更によって変更が生じるので改善設計策求
    /// </summary>
    private void Init_status()
    {
        if (base_status == null) return; //何かしら例外処理を行うべきかも

        MAXHP = base_status.MAX_HP;
        hp = base_status.HP;
        atk = base_status.ATK;
        matk = base_status.MATK;
        def = base_status.DEF;
        mdef = base_status.MDEF;
        ms = base_status.MS;

        shield = base_status.SHIELD;
        mshield = base_status.MSHIELD;

        ///TODO バフ/デバフ/アイテム,スキル効果を保持するリストを初期化する
    }

    ///TODO 効果保持リストに要素を追加、削除、ステータスに適応などの実装
}

