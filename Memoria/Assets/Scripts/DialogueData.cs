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

    [Space(20)]
    public Transform elenaLookAtPoint;

    [Header("Quest icon")]
    public bool moveQuestIconAfterConversation;
    public Transform questIconPosition;
    public Vector3 questIconOffset;

    [Header("Move actor")]
    public bool moveCharacterAfterConversation;
    public Transform newCharacterPosition;

    [Space(20)]
    public bool disableActorAfterDialogue;

    [Space(20)]
    public UnityEvent conversationStart;
    public UnityEvent conversationEnd;

}
