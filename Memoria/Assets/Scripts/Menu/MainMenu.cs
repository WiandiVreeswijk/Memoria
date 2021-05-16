using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;

public class MainMenu : MonoBehaviour
{
    public PlayableDirector director;
    public GameObject controlPanel;

    public CinemachineVirtualCamera mainMenuCamera; 
    public void PlayGame()
    {
        StartTimeLine();
        mainMenuCamera.Priority = 10;
    }
    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
    public void StartTimeLine()
    {
        director.Play();
    }
}
