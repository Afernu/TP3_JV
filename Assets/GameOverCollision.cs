using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //Si la collisio est faite par l'ennemi, loader la scene de game over
        if (other.CompareTag("Enemy"))
        {
            IncreasingTime increaseTime = FindObjectOfType<IncreasingTime>();

            increaseTime.GameOver();

            SceneManager.LoadScene("GameOver");

        }
    }
}
