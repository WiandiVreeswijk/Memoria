using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using PixelCrushers.DialogueSystem;
using UnityEngine;

public class DialogueHandlerOma : MonoBehaviour, IDialogueHandler {
    public List<DialogueData> dialogueData = new List<DialogueData>();

    public Transform teleportPoint;
    public Transform omaWoonkamerPoint;
    public Transform watchPoint;
    public GameObject watchObject;
    public GameObject fakeElena;

    private CinemachineCameraPriorityOnDialogueEvent dialogueCameraPriority;
    private Icon questIcon;
    public void Start() {
        dialogueCameraPriority = GetComponent<CinemachineCameraPriorityOnDialogueEvent>();
        questIcon = Globals.IconManager.AddWorldIcon("oma", transform.position + new Vector3(0, 2.25f, 0));
        watchObject.SetActive(false);
    }

    private DialogueData GetDialogueDataFromConversation(string name) {
        foreach (var data in dialogueData) {
            if (data.dialogueName == name) return data;
        }

        throw new Exception("No dialogue data found for conversation: " + name);
    }

    public void ConversationStart(string conversationName, GameObject conversationPlayer) {
        print("Conversation with oma started: " + conversationName);
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
        print("Conversation with oma ended: " + conversationName);
        fakeElena.SetActive(false);

        DialogueData data = GetDialogueDataFromConversation(conversationName);
        if (data != null) {
            data.conversationStart.Invoke();
            if (data.moveCharacterAfterConversation) {
                transform.SetPositionAndRotation(data.newCharacterPosition);
            }

            if (data.moveQuestIconAfterConversation) {
                Vector3 pos = data.questIconPosition == null
                    ? transform.position + data.questIconOffset
                    : data.questIconPosition.position;
                questIcon.SetPosition(pos);
            }
        }
    }

    public void ConversationLine(string conversationName, string line, GameObject conversationPlayer) {

    }
}