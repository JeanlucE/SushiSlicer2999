using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class HighscoreScreen : MonoBehaviour 
{
    public Text scoreDisplay;

    void Start()
    {
        float score = ScoreCanvas.lastHighscore;
        scoreDisplay.text = "" + score;

        StartCoroutine(loadMainMenu(10.0f));
    }

    private IEnumerator loadMainMenu(float delay)
    {
        yield return new WaitForSeconds(delay);

        SceneManager.LoadScene("title");
    }
}
