using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class preserveState : MonoBehaviour
{

    public List<GameObject> gameObjects;

    // Start is called before the first frame update
    void Start()
    {
        foreach(GameObject obj in gameObjects){
            DontDestroyOnLoad(obj);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
