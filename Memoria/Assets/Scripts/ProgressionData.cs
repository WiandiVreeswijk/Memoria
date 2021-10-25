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

    public void Progress() {
        if (setActorPosition) {
            actor.transform.position = newActorPosition.position;
            actor.transform.rotation = newActorPosition.rotation;
        }

        Globals.ProgressionManager.GetIcon().SetEnabled(setQuestIcon);
        if (setQuestIcon)
            Globals.ProgressionManager.GetIcon().transform.position = questIconPosition.position + questIconOffset;
        if (setDialogueProgression) {
            DialogueLua.SetVariable(actorName + "_Progression", dialogueProgressionName);
            actor.GetComponent<Usable>().enabled = true;
        }
    }

    public void ActivateEnter() {
        if (shouldTriggerOnActivatable) {
            Progress();
        }
    }
}
