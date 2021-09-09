using System;
using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using DG.Tweening;
using UnityEngine;

public class DialogueSystemHandler : MonoBehaviour {
    private string activeConversation;
    private IDialogueHandler activeHandler;
    public void OnConversationStart(UnityEngine.Object actor) {
        if (actor.GetType() == typeof(Transform)) {
            activeHandler = ((Transform)actor).GetComponent<IDialogueHandler>();
            if (activeHandler != null) {
                activeConversation = DialogueLua.GetVariable("Progression").AsString;
                activeHandler.ConversationStart(activeConversation);
                Globals.Player.PlayerMovementAdventure.Teleport(activeHandler.GetElenaConversationTransform().position);
                Globals.Player.transform.rotation = activeHandler.GetElenaConversationTransform().rotation;
            } else throw new Exception("No dialogue handler found on " + actor.name);
        }
    }

    public void OnConversationEnd(UnityEngine.Object actor) {
        if (actor.GetType() == typeof(Transform)) {
            IDialogueHandler handler = ((Transform)actor).GetComponent<IDialogueHandler>();
            if (activeHandler != handler) throw new Exception("Active handler does not equal conversation end handler");
            if (handler != null) {
                handler.ConversationEnd(activeConversation);
                activeConversation = "";
                activeHandler = null;
            } else throw new Exception("No dialogue handler found on " + actor.name);
        }
    }

    public void OnConversationLine(Subtitle subtitle) {
        if (activeHandler == null) throw new Exception("No active conversation handler");
        activeHandler.ConversationLine(activeConversation, subtitle.formattedText.text);
    }
}
