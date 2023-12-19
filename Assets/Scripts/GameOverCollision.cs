using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverCollision : MonoBehaviour
{
    //IncreasingTime increaseTime = FindObjectOfType<IncreasingTime>();

    private void OnTriggerEnter(Collider other)
    {
        //Si la collision est faite par l'ennemi, loader la scene de game over
        if (other.CompareTag("Enemy"))
        {

            //increaseTime.GameOver();

            SceneManager.LoadScene("GameOver");

        }
    }
}
