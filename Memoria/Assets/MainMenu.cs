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
    private void Awake()
    {
        director.played += Director_Played;
        director.stopped += Director_Stopped;
    }
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
    private void Director_Played(PlayableDirector obj)
    {
        controlPanel.SetActive(true);
    }

    private void Director_Stopped(PlayableDirector obj)
    {
        controlPanel.SetActive(true);
    }

    public void StartTimeLine()
    {
        director.Play();
    }
}
