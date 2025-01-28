using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class loadMainScene : MonoBehaviour
{
    public string sceneToLoad = "GameScene"; // The scene you want to preload
    AsyncOperation operation;
    void Start()
    {
            operation = SceneManager.LoadSceneAsync(sceneToLoad);
            operation.allowSceneActivation = false;
    }

    private void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.CompareTag("player")){
            Debug.Log(operation.progress);
            operation.allowSceneActivation = true;
            
        }
    }
    void Update(){
        
    }
}
