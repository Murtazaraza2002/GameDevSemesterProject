using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public GameObject QuitTextObject;
    // Start is called before the first frame update
    void Start()
    {

        QuitTextObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.Escape))
        {
            Debug.Log("Exiting the game...");
            QuitTextObject.SetActive(true);
            Invoke("DelayedAction", 2f);
        }
    }
    public void PlayGame()
    {
        SceneManager.LoadScene("MainGame");
    }
    public void QuitGame()
    {
        Debug.Log("Exiting Game...");
        QuitTextObject.SetActive(true);
        Invoke("DelayedAction", 2f);
        Application.Quit();
    }
    public void LoadGuide()
    {
        SceneManager.LoadScene("GameGuide");
    }
    private void DelayedAction()
    {
        Application.Quit();
    }
}
