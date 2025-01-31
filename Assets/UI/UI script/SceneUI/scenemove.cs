using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class scenemove : MonoBehaviour
{
    // start game button
    public void startgame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }

    // go back to main page(from game)
    public void endgame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void nextleveltodesert()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(2);
    }
    public void nextleveltoiceland()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(3);
    }
}
