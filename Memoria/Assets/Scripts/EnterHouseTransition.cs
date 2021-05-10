using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class EnterHouseTransition : MonoBehaviour
{
    //public VideoPlayer videoPlayer;
    public GameObject elenaMesh;

    private bool playerEntered = false;

    private void Start()
    {
        //videoPlayer.Stop();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!playerEntered)
        {
            if (other.gameObject.tag == "Player")
            {
                playerEntered = true;
                //videoPlayer.Play();
            }
        }
        else
        {
            playerEntered = false;
            //videoPlayer.Pause();
        }
        elenaMesh.SetActive(!playerEntered);
    }
}
