using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDialogueHandler {
    public void OnConversationStart(string conversationName);
    public void OnConversationEnd(string conversationName);
    public Transform GetElenaConversationTransform();
}
