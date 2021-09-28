using System.Collections;
using System.Collections.Generic;
using PixelCrushers.DialogueSystem;
using UnityEngine;
using UnityEngine.Events;

public class ProgressionData : MonoBehaviour, IEnterActivatable {
    [SerializeField] private bool setActorPosition;
    [SerializeField] private GameObject actor;
    [SerializeField] private Transform newActorPosition;

    [Space(20)]
    [SerializeField] private bool setQuestIcon;
    [SerializeField] private Transform questIconPosition;
    [SerializeField] private Vector3 questIconOffset;

    [Space(20)]
    [SerializeField] private bool setDialogueProgression;
    [SerializeField] private string actorName;
    [SerializeField] private string dialogueProgressionName;

    [Space(20)]
    [SerializeField] private bool shouldTriggerOnActivatable;

    public UnityEvent onProgression;

    void Progress() {
        if (setActorPosition) {
            actor.transform.position = newActorPosition.position;
            actor.transform.rotation = newActorPosition.rotation;
        }

        if (setQuestIcon) {
            Globals.ProgressionManager.GetIcon().transform.position = questIconPosition.position + questIconOffset;
            Globals.ProgressionManager.GetIcon().SetEnabled(true);
        } else Globals.ProgressionManager.GetIcon().SetEnabled(false);

        if (setDialogueProgression) {
            DialogueLua.SetVariable(actorName + "_Progression", dialogueProgressionName);
        }
    }

    public void ActivateEnter() {
        if (shouldTriggerOnActivatable) {
            Progress();
        }
    }
}
