using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Cette fonction est appelée lorsque le bouton "Play" est cliqué
    public void PlayGame()
    {
        // Charge la scène "Cinematique"
        SceneManager.LoadScene("Cinematique");
    }

    // Quitte le jeu (optionnel, pour un bouton Quit)
    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}