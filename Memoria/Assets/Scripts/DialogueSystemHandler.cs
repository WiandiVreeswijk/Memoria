using System;
using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using DG.Tweening;
using UnityEngine;

public class DialogueSystemHandler : MonoBehaviour {
    public GameObject conversationPlayer;
    private string activeConversation;
    private IDialogueHandler activeHandler;

    //public void Start()
    //{
    //    DialogueSystemEvents events = GetComponent<DialogueSystemEvents>();
    //    events.conversationEvents.onConversationStart
    //}
    public void OnConversationStart(UnityEngine.Object actor) {
        if (actor.GetType() == typeof(Transform)) {
            activeHandler = ((Transform)actor).GetComponent<IDialogueHandler>();
            if (activeHandler != null) {
                //Stop the player from moving and disable visibility
                Globals.Player.CameraController.SetInputEnabled(false);
                Globals.Player.PlayerMovementAdventure.SetCanMove(false);
                Globals.Player.VisualEffects.SetVisible(false);

                Lua.Result result = DialogueLua.GetVariable(activeHandler.GetActorName() + "_Progression");
                activeConversation = result.IsString ? result.AsString : "";
                KeyValuePair<Vector3, Quaternion>? positionData = activeHandler.ConversationStart(activeConversation, conversationPlayer);
                if (positionData.HasValue) {
                    Globals.Player.transform.position = positionData.Value.Key;
                    Globals.Player.transform.rotation = positionData.Value.Value;
                }

            } else throw new Exception("No dialogue handler found on " + actor.name);
        }
    }

    public void OnConversationEnd(UnityEngine.Object actor) {
        if (actor.GetType() == typeof(Transform)) {
            IDialogueHandler handler = ((Transform)actor).GetComponent<IDialogueHandler>();
            if (activeHandler != handler) throw new Exception("Active handler does not equal conversation end handler");
            if (handler != null) {
                //Return movement and visibility
                Globals.Player.CameraController.SetInputEnabled(true);
                Globals.Player.PlayerMovementAdventure.SetCanMove(true);
                Globals.Player.VisualEffects.SetVisible(true);

                activeConversation = "";
                activeHandler = null;

                handler.ConversationEnd(activeConversation, conversationPlayer);
            } else throw new Exception("No dialogue handler found on " + actor.name);
        }
    }

    public void OnConversationLine(Subtitle subtitle) {
        if (activeHandler == null) throw new Exception("No active conversation handler");
        activeHandler.ConversationLine(activeConversation, subtitle.formattedText.text, conversationPlayer);
    }
}
