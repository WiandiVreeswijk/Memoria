using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDialogueHandler {
    public void ConversationStart(string conversationName);
    public void ConversationEnd(string conversationName);
    public void ConversationLine(string conversationName, string line);
    public Transform GetElenaConversationTransform();
}
