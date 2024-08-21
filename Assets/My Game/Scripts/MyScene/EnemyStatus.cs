using System.Collections;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 敵の状態管理スクリプト
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyStatus : MobStatus
{
    public float enemyLifetime = 120f;
    private NavMeshAgent _agent;

    protected override void Start()
    {
        base.Start();

        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        // NavMeshAgentのvelocityで移動速度のベクトルが取得できる
        _animator.SetFloat("MoveSpeed", _agent.velocity.magnitude);
    }

    public override void OnDie()
    {
        base.OnDie();
        StartCoroutine(DestroyCoroutine());
    }

    /// <summary>
    /// 倒された時の消滅コルーチンです。
    /// </summary>
    /// <returns></returns>
    private IEnumerator DestroyCoroutine()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);

        // 死亡時の座標
        Vector3 deathPosition = transform.position;

        // 矢印のPrefabが設定されているか確認
        if (arrowPrefab != null)
        {
            //treasureの位置を取得し向きを割り出す
            SystemManager systemManager = GameObject.Find("/SystemManager").GetComponent<SystemManager>();
            Vector3 direction = deathPosition - systemManager.spawnPos;
            direction.y = 0;

            // 矢印のGameObjectを生成してCanvas上に表示
            DisplayArrowAtPosition(deathPosition, direction); //TODO 矢印向き
        }
        else
        {
            Debug.LogError("Arrow Prefabが設定されていません。");
        }

    }

    //指定された矢印prefabを指定ベクトルの向きに配置
    public GameObject arrowPrefab; // 矢印のPrefabをInspectorから指定
    void DisplayArrowAtPosition(Vector3 position, Vector3 direction)
    {
        // 矢印のGameObjectを生成
        GameObject arrow = Instantiate(arrowPrefab, position, Quaternion.LookRotation(direction));

        //TODO? 距離を示唆する色設定
    }


    /// <summary>
    /// 自然消滅コルーチンです。生成時(CreateEnemy)にコルーチンを始動させます。
    /// </summary>
    public IEnumerator DeathCoroutine()
    {
        yield return new WaitForSeconds(enemyLifetime);
        //Debug.Log(transform.root.gameObject + " destroyed!");
        Destroy(transform.root.gameObject);
    }
}