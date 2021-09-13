using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using PixelCrushers.DialogueSystem;
using UnityEngine;

public class DialogueHandlerOma : MonoBehaviour, IDialogueHandler {
    public Transform teleportPoint;
    public Transform omaWoonkamerPoint;
    public Transform watchPoint;
    public GameObject watchObject;
    private CinemachineCameraPriorityOnDialogueEvent dialogueCameraPriority;
    private Icon questIcon;
    public void Start() {
        dialogueCameraPriority = GetComponent<CinemachineCameraPriorityOnDialogueEvent>();
        questIcon = Globals.IconManager.AddWorldIcon("oma", transform.position + new Vector3(0, 2.25f, 0));
        watchObject.SetActive(false);
    }
    public void ConversationStart(string conversationName, GameObject conversationPlayer) {
        print("Conversation with oma started: " + conversationName);

        if (conversationName == "watch") {
            dialogueCameraPriority.virtualCamera = omaWoonkamerPoint.GetComponentInChildren<CinemachineVirtualCamera>();
        }
    }

    public void ConversationEnd(string conversationName, GameObject conversationPlayer) {
        print("Conversation with oma ended: " + conversationName);

        if (conversationName == "introduction") {
            //Move grandma to the living room
            transform.SetPositionAndRotation(omaWoonkamerPoint);
            questIcon.SetPosition(transform.position + new Vector3(0, 2.25f, 0));
        } else if (conversationName == "watch") {
            watchObject.SetActive(true);
            questIcon.SetPosition(watchPoint.position + new Vector3(0, 2.25f, 0));
        }
    }

    public void ConversationLine(string conversationName, string line, GameObject conversationPlayer) {
    }

    public Transform GetElenaConversationTransform() {
        return Globals.Player.transform;
        return teleportPoint;
    }
}