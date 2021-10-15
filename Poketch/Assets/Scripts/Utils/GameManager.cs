using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager get;

    public Camera mainCam;

    private void Awake()
    {
        get = this;
    }

    public void QuitApp()
    {
        Application.Quit();
    }
}
