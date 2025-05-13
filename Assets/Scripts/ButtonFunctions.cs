using UnityEngine;

public class ButtonFunctions : MonoBehaviour
{
    public void LoadScene(string sceneName)//carga la escena a la que el boton este asignada
    {
        GameManager.instance.LoadScene(sceneName);
    }

    public void Play()
    {
        GameManager.instance.ReadyToPlay();
        Invoke("LoadLevel", 3f);
    }

    public void ExitGame()//hace que salga del juego
    {
        GameManager.instance.ReadyToExit();
    }

    public void LoadLevel()
    {
        GameManager.instance.LoadScene("Tests");
    }
}
