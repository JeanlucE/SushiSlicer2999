using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TitleMenu : MonoBehaviour 
{
    public AudioSource audioSource;
    public AudioClip sliceSound, haiSound;

    public void OnStartGameButton()
    {
        audioSource.PlayOneShot(haiSound);
        audioSource.PlayOneShot(sliceSound);
        
        StartCoroutine(StartGame(0.5f));
    }

    public void OnEndGameButton()
    {
        audioSource.PlayOneShot(haiSound);
        audioSource.PlayOneShot(sliceSound);

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
