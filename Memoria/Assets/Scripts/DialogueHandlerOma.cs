using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using PixelCrushers.DialogueSystem;
using UnityEngine;

public class DialogueHandlerOma : MonoBehaviour, IDialogueHandler {
    public Transform teleportPoint;
    public Transform omaWoonkamerPoint;
    private CinemachineCameraPriorityOnDialogueEvent dialogueCameraPriority;
    private Icon questIcon;
    public void Start() {
        dialogueCameraPriority = GetComponent<CinemachineCameraPriorityOnDialogueEvent>();
        questIcon = Globals.IconManager.AddWorldIcon("oma", transform.position + new Vector3(0, 2.25f, 0));
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
            transform.SetPositionAndRotation(omaWoonkamerPoint);
            questIcon.SetPosition(transform.position + new Vector3(0, 2.25f, 0));
        } else if (conversationName == "watch") {
            transform.DORotate(new Vector3(0, 720, 0), 10.0f, RotateMode.FastBeyond360);
        }
    }

    public void ConversationLine(string conversationName, string line, GameObject conversationPlayer) {
    }

    public Transform GetElenaConversationTransform() {
        return Globals.Player.transform;
        return teleportPoint;
    }
}