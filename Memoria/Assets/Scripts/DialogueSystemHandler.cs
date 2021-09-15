using System;
using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using DG.Tweening;
using UnityEngine;

public class DialogueSystemHandler : MonoBehaviour
{
    public GameObject conversationPlayer;
    private string activeConversation;
    private IDialogueHandler activeHandler;
    public void OnConversationStart(UnityEngine.Object actor) {
        if (actor.GetType() == typeof(Transform)) {
            activeHandler = ((Transform)actor).GetComponent<IDialogueHandler>();
            if (activeHandler != null)
            {
                Lua.Result result = DialogueLua.GetVariable(activeHandler.GetActorName() + "_Progression");
                activeConversation = result.IsString ? result.AsString : "";
                activeHandler.ConversationStart(activeConversation, conversationPlayer);
            } else throw new Exception("No dialogue handler found on " + actor.name);
        }
    }

    public void OnConversationEnd(UnityEngine.Object actor) {
        if (actor.GetType() == typeof(Transform)) {
            IDialogueHandler handler = ((Transform)actor).GetComponent<IDialogueHandler>();
            if (activeHandler != handler) throw new Exception("Active handler does not equal conversation end handler");
            if (handler != null) {
                handler.ConversationEnd(activeConversation, conversationPlayer);
                activeConversation = "";
                activeHandler = null;
            } else throw new Exception("No dialogue handler found on " + actor.name);
        }
    }

    public void OnConversationLine(Subtitle subtitle) {
        if (activeHandler == null) throw new Exception("No active conversation handler");
        activeHandler.ConversationLine(activeConversation, subtitle.formattedText.text, conversationPlayer);
    }
}
