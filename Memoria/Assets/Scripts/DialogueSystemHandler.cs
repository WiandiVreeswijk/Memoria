using System;
using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using DG.Tweening;
using UnityEngine;

public class DialogueSystemHandler : MonoBehaviour
{
    private string activeConversation;
    public void OnConversationStart(UnityEngine.Object actor) {
        if (actor.GetType() == typeof(Transform)) {
            IDialogueHandler handler = ((Transform)actor).GetComponent<IDialogueHandler>();

            if (handler != null) {
                activeConversation = DialogueLua.GetVariable("Progression").AsString;
                handler.OnConversationStart(activeConversation);
                Globals.Player.PlayerMovementAdventure.Teleport(handler.GetElenaConversationTransform().position);
                Globals.Player.transform.rotation = handler.GetElenaConversationTransform().rotation;
            } else throw new Exception("No dialogue handler found on " + actor.name);
        }
    }

    public void OnConversationEnd(UnityEngine.Object actor) {
        if (actor.GetType() == typeof(Transform)) {
            IDialogueHandler handler = ((Transform)actor).GetComponent<IDialogueHandler>();
            if (handler != null) {
                handler.OnConversationEnd(activeConversation);
                activeConversation = "";
            } else throw new Exception("No dialogue handler found on " + actor.name);
        }
    }
}
