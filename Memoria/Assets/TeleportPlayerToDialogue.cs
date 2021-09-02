using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPlayerToDialogue : MonoBehaviour
{
    public void OnConversationStart(UnityEngine.Object actor)
    {
        if (actor.GetType() == typeof(Transform))
        {
            DialogueTeleportData data = ((Transform)actor).GetComponent<DialogueTeleportData>();
            if (data != null) {
                Globals.Player.transform.position = data.teleportPoint.transform.position;
                Globals.Player.transform.rotation = data.teleportPoint.transform.rotation;
            }
        }
    }
}
