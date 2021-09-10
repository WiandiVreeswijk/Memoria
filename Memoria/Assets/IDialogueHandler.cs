using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDialogueHandler {
    public void ConversationStart(string conversationName, GameObject conversationPlayer);
    public void ConversationEnd(string conversationName, GameObject conversationPlayer);
    public void ConversationLine(string conversationName, string line, GameObject conversationPlayer);
    public Transform GetElenaConversationTransform();
}
