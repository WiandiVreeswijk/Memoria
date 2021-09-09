using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueHandlerOma : MonoBehaviour, IDialogueHandler {
    public Transform teleportPoint;

    public void OnConversationStart(string conversationName) {
        print("Conversation with oma started: " + conversationName);
    }

    public void OnConversationEnd(string conversationName) {
        print("Conversation with oma ended: " + conversationName);
    }

    public Transform GetElenaConversationTransform() {
        return teleportPoint;
    }
}
