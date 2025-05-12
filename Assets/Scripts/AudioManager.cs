using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    private List<GameObject> audioList;

    void Awake()
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
        audioList = new List<GameObject>();
    }

    public AudioSource PlayAudio(AudioClip audioClip, string gameObjectName, bool isLoop = true, float volume = 1.0f)//le das valores al la pista de audio
    {
        GameObject audioObject = new GameObject(gameObjectName);
        audioObject.transform.SetParent(transform);
        AudioSource srcComponent = audioObject.AddComponent<AudioSource>();
        srcComponent.clip = audioClip;
        srcComponent.volume = volume;
        srcComponent.loop = isLoop;
        srcComponent.Play();
        audioList.Add(audioObject);



        return srcComponent;
    }

    public void ClearAudio() //limpia el audio de la pantalla
    {
        foreach (GameObject audioObject in audioList)
        {
            Destroy(audioObject);
        }
        audioList.Clear();
    }
}
