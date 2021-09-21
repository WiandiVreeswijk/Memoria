using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;

//Data used at conversation start/end
public class DialogueData : MonoBehaviour
{
    public string dialogueName;
    public Transform fakeElenaPoint;
    public CinemachineVirtualCamera cam;

    public Transform elenaLookAtPoint;

    public bool moveQuestIconAfterConversation;
    public Transform questIconPosition;
    public Vector3 questIconOffset;

    public bool moveCharacterAfterConversation;
    public Transform newCharacterPosition;

    public UnityEvent conversationStart;
    public UnityEvent conversationEnd;
}
