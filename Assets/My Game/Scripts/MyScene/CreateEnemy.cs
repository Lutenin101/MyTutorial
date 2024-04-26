using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class CreateEnemy : MonoBehaviour
{
    [SerializeField] private int _width = 00;  //生成範囲(立方体,その一辺)
    [SerializeField] private float _spawnCooldown = 5f; //生成間隔(*1~*3)
    [SerializeField] public int _maxNum = 1; //1ループ内の生成数
    [SerializeField] GameObject enemy; //生成する敵 今の所一種類

    //private Terrain terrain; //Terrain コンポーネントへの参照
    private NavMeshSurface navMeshSurface; // NavMeshSurface コンポーネントへの参照

    // Use this for initialization
    void Start()
    {
        //Terrain terrain = Terrain.activeTerrain;
        navMeshSurface = FindObjectOfType<NavMeshSurface>(); // NavMeshSurface コンポーネントを検索して取得する

        StartCoroutine(SpawnLoop()); ///TODO ゲーム開始時(タイトル切り替え時)にコルーチンを開始させる
    }


    // Update is called once per frame
    void Update()
    {
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {

            for (int i = 0; i < _maxNum; i++)
            {
                //terain上の _width * _width の範囲のどこかに生成する
                //Vector3 spawnPos;
                //const float posyLoss = -1.0f; //地面上に出すための差分

                //float posx = Random.Range(-_width / 2, _width / 2);
                //float posz = Random.Range(-_width / 2, _width / 2);

                ////x,y座標からterrain上の地表座標を取得
                //float posy = terrain.terrainData.GetInterpolatedHeight(
                //    posx / terrain.terrainData.size.x,
                //    posz / terrain.terrainData.size.z) + terrain.transform.position.y + posyLoss;

                //spownPos = new Vector3(posx, posy, posz);

                // NavMesh 上の半径 _width の円の範囲のどこかに生成する
                Vector3 enemySpawnPos = GetRandomNavMeshPosition();

                var obst = Instantiate(enemy, enemySpawnPos, transform.rotation);
                //Debug.Log("Instantiate Enemy.");

                //Caution: enemyのオブジェクト階層構造に注意。2024/3/28現在、skeltonのみ対応。
                // Skeleton Body オブジェクトから EnemyStatus コンポーネントを取得して自然消滅コルーチンを開始する
                var enemyStatus = obst.GetComponentInChildren<EnemyStatus>();
                if (enemyStatus != null)
                {
                    StartCoroutine(enemyStatus.DeathCoroutine()); // 自然消滅コルーチンを開始する
                }
                else
                {
                    Debug.LogError("EnemyStatus not found in Skeleton Body.");
                }

                yield return new WaitForSeconds(Random.Range(_spawnCooldown, _spawnCooldown * 3));
            }
        }
        //TODO 経過時間に応じて敵キャラのHPを増やすなど
    }

    private Vector3 GetRandomNavMeshPosition()
    {
        // NavMesh 上のランダムな位置を取得する
        NavMeshHit hit;
        Vector3 randomPosition = Vector3.zero;
        bool found = false;

        // NavMesh 上のランダムな位置を取得するまで繰り返す
        while (!found)
        {
            Vector3 randomDirection = Random.insideUnitSphere * _width;
            randomDirection += transform.position;
            found = NavMesh.SamplePosition(randomDirection, out hit, _width, NavMesh.AllAreas);
            if (found)
            {
                randomPosition = hit.position;
            }
        }

        return randomPosition;
    }

}
