using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour
{
    public KeyCode restartKey = KeyCode.Space;

    private void Update()
    {
        if (Input.GetKeyUp(restartKey))
            SceneManager.LoadScene("Game");
    }
}
