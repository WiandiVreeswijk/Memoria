using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDialogueHandler {
    //Return type is either null or the position and rotation where the player has to teleport to
    public abstract KeyValuePair<Vector3, Quaternion>? ConversationStart(string conversationName, GameObject conversationPlayer);
    public abstract void ConversationEnd(string conversationName, GameObject conversationPlayer);
    public abstract void ConversationLine(string conversationName, string line, GameObject conversationPlayer);
    public abstract string GetActorName();
}
