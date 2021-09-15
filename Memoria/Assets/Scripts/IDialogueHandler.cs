using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDialogueHandler {
    public abstract void ConversationStart(string conversationName, GameObject conversationPlayer);
    public abstract void ConversationEnd(string conversationName, GameObject conversationPlayer);
    public abstract void ConversationLine(string conversationName, string line, GameObject conversationPlayer);
    public abstract string GetActorName();
}
