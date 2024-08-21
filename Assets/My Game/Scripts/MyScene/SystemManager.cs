using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

/// <summary>
/// ゲームシステムに関わるマネジメントスクリプト
/// </summary>
public class SystemManager : MonoBehaviour
{
    [SerializeField] GameObject treasure;
    public Vector3 spawnPos; //treasureの位置

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("SystemManager.cs started.");
        InstantiateTreasure();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InstantiateTreasure()
    {
        Terrain terrain = Terrain.activeTerrain;

        //terain上の posxMax * poszMax の範囲のどこかに宝を埋める
        const int posxMax = 500;
        const int poszMax = 500;
        const float posyLoss = -1.0f; //地面に埋めるための誤差

        float posx = Random.Range(-posxMax / 2, posxMax / 2);
        float posz = Random.Range(-poszMax / 2, poszMax / 2);

        //x,y座標からterrain上の地表座標を取得
        float posy = terrain.terrainData.GetInterpolatedHeight(
            posx / terrain.terrainData.size.x,
            posz / terrain.terrainData.size.z) + terrain.transform.position.y + posyLoss;

        spawnPos = new Vector3(posx, posy, posz);

        Debug.Log("The treasure was buried. At: " + spawnPos);

        var obst = Instantiate(treasure, spawnPos, transform.rotation);

    }
}
