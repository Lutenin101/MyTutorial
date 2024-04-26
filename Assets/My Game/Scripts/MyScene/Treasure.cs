using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Treasure : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int RayLength = 500;
        Debug.DrawRay(this.transform.position, Vector3.up * RayLength, Color.red, 50.0f, false);
    }


    public void Detected()
    {
        Debug.Log("Detected");
        // シーン遷移の際にはSceneManagerを使用する
        SceneManager.LoadScene("ClearScene");
        //SceneManager.LoadScene(0);
    }

}
