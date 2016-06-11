using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TitleMenu : MonoBehaviour 
{
    public void OnStartGameButton()
    {
        StartCoroutine(StartGame(0.5f));
    }

    public void OnEndGameButton()
    {
        StartCoroutine(QuitGame(0.5f));
    }

    private IEnumerator QuitGame(float delay)
    {
        yield return new WaitForSeconds(delay);

        Application.Quit();
    }

    private IEnumerator StartGame(float delay)
    {
        yield return new WaitForSeconds(delay);

        SceneManager.LoadScene("roberttest");
    }
}
