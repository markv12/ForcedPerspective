using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class DebugControls : MonoBehaviour {

	void Update () {
        if (Input.GetKeyDown(KeyCode.R)) {
            SceneManager.LoadScene("Main");
            
        } else if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Q)) {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
	}
}
