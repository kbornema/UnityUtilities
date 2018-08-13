using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneRefTest : MonoBehaviour
{   
    [SerializeField]
    private SceneRef _sceneRef;
	
	// Update is called once per frame
	private void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            _sceneRef.LoadScene();
    }
}
