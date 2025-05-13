using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; //el game manager controla las variables del juego y es accesible a todos
    private float time;
    private int points;
    private KeyCode Esc = KeyCode.Escape;
    //public AudioClip SelectClip;

    public bool readyToPlay = false;
    public bool readyToExit = false;

    public enum GameManagerVariables { TIME, POINTS };//para facilitar el codigo

    private void Awake()
    {
        if (!instance)
        {
            instance = this;//se instancia el objecto
            DontDestroyOnLoad(gameObject);// no se destruye entre cargas
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Update()
    {
        time += Time.deltaTime;
        if (Input.GetKeyDown(Esc))
        {
            readyToPlay = false;
            SceneManager.LoadScene("Menu");
            //AudioManager.instance.ClearAudio();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            readyToPlay = false;
            time = 0;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            //AudioManager.instance.ClearAudio();
        }
    }

    // getter
    public float GetTime()
    {
        return time;
    }

    // getter
    public int GetPoints()
    {
        return points;
    }

    // setter
    public void SetPoints(int value)
    {
        points = value;
    }

    public void LoadScene(string sceneName)
    {
        time = 0;
        SceneManager.LoadScene(sceneName);
        //AudioManager.instance.ClearAudio();
    }

    public void ReadyToPlay()
    {
        readyToPlay = true;
    }

    public void ReadyToExit()
    {
        readyToExit = true;
        Invoke("ExitGame", 2f);
    }

    public void ExitGame()
    {
        Debug.Log("EXIT");
        Application.Quit();
    }

    internal void LoadScene(object sceneName)
    {
        throw new NotImplementedException();
    }
}