using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using PixelCrushers.DialogueSystem;
using UnityEngine;

public class DialogueHandler : MonoBehaviour, IDialogueHandler {
    public List<DialogueData> dialogueData = new List<DialogueData>();
    public string actorName;
    public GameObject fakeElena;

    private CinemachineCameraPriorityOnDialogueEvent dialogueCameraPriority;
    public void Start() {
        dialogueCameraPriority = GetComponent<CinemachineCameraPriorityOnDialogueEvent>();
        actorName = actorName.ToLower();
        //DialogueLua.GetVariable(actorName + "_Progression");
    }

    private DialogueData GetDialogueDataFromConversation(string name) {
        foreach (var data in dialogueData) {
            if (data.dialogueName == name) return data;
        }

        throw new Exception("No dialogue data found for conversation: " + name);
    }

    public void ConversationStart(string conversationName, GameObject conversationPlayer) {
        print("Conversation with " + actorName + " started" + (conversationName.Length > 0 ? ": " + conversationName : "."));
        fakeElena.SetActive(true);

        DialogueData data = GetDialogueDataFromConversation(conversationName);
        if (data != null) {
            fakeElena.transform.SetPositionAndRotation(data.fakeElenaPoint);
            Globals.Player.PlayerMovementAdventure.Teleport(data.fakeElenaPoint.position);
            dialogueCameraPriority.virtualCamera = data.cam;
            data.conversationStart.Invoke();
        }
    }

    public void ConversationEnd(string conversationName, GameObject conversationPlayer) {
        print("Conversation with " + actorName + " ended" + (conversationName.Length > 0 ? ": " + conversationName : "."));
        fakeElena.SetActive(false);

        DialogueData data = GetDialogueDataFromConversation(conversationName);
        if (data != null) {
            data.conversationEnd.Invoke();
            if (data.moveCharacterAfterConversation) {
                transform.SetPositionAndRotation(data.newCharacterPosition);
            }

            if (data.moveQuestIconAfterConversation) {
                Vector3 pos = data.questIconPosition == null
                    ? transform.position + data.questIconOffset
                    : data.questIconPosition.position + data.questIconOffset;
                Globals.ProgressionManager.GetIcon().SetPosition(pos);
            }
        }
    }

    public void ConversationLine(string conversationName, string line, GameObject conversationPlayer) {

    }

    public string GetActorName() {
        return actorName;
    }
}