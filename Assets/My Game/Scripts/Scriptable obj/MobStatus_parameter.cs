using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Create StatusData")]

/// <summary>
/// Mobのステータス情報,変数宣言とアクセッサ
/// MEMO: ステータスの数を変更する場合、Status_Modifierも変更する必要あり
/// </summary>
public class MobStatus_parameter : ScriptableObject
{

    [SerializeField] private static int MAXHP = 10000;//最大体力
    [SerializeField] private int hp;//現在体力
    [SerializeField] private int atk; //物理攻撃力
    [SerializeField] private int matk;//魔法攻撃力
    [SerializeField] private int def; //物理防御力
    [SerializeField] private int mdef;//魔法防御力
    [SerializeField] private int ms;    //移動スピード


    [SerializeField] private int shield; //物理障壁
    [SerializeField] private int mshield;//魔法障壁


    //アクセッサ
    public int MAX_HP
    {
        get { return MAXHP; }
    }

    public int HP
    {
        get { return hp; }
    }

    public int ATK
    {
        get { return atk; }
    }

    public int MATK
    {
        get { return matk; }
    }

    public int DEF
    {
        get { return def; }
    }

    public int MDEF
    {
        get { return mdef; }
    }

    public int MS
    {
        get { return ms; }
    }

    public int SHIELD
    {
        get { return shield; }
    }

    public int MSHIELD
    {
        get { return mshield; }
    }
}

