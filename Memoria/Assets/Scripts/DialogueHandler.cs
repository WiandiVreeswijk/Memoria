using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using FMODUnity;
using PixelCrushers.DialogueSystem;
using UnityEngine;
using Random = UnityEngine.Random;

public class DialogueHandler : MonoBehaviour, IDialogueHandler {
    public List<DialogueData> dialogueData = new List<DialogueData>();
    public string actorName;
    public GameObject fakeElena;

    private ActorIdleSound actorIdleSound;

    [Space(20)]
    [EventRef]
    public string soundEffectStart;

    [EventRef]
    public string soundEffectEnd;

    public void Start() {
        actorName = actorName.ToLower();
        //DialogueLua.GetVariable(actorName + "_Progression");

        if (actorIdleSound != null) actorIdleSound = GetComponent<ActorIdleSound>();
    }

    private DialogueData GetDialogueDataFromConversation(string name) {
        foreach (var data in dialogueData) {
            if (data.dialogueName == name) return data;
        }

        throw new Exception("No dialogue data found for conversation: " + name);
    }

    public KeyValuePair<Vector3, Quaternion>? ConversationStart(string conversationName, GameObject conversationPlayer) {
        print("Conversation with " + actorName + " started" + (conversationName.Length > 0 ? ": " + conversationName : "."));
        //fakeElena.SetActive(true);

        DialogueData data = GetDialogueDataFromConversation(conversationName);
        if (data != null) {
            data.cam.Priority = 99;
            fakeElena.transform.position = data.fakeElenaPoint.position + new Vector3(0.0f, 0.05f, 0.0f);
            fakeElena.transform.rotation = data.fakeElenaPoint.rotation;
            Globals.Player.VisualEffects.SetLookAt(data.elenaLookAtPoint == null ? (Vector3?)null : data.elenaLookAtPoint.position);
            Globals.Player.PlayerMovementAdventure.Teleport(data.fakeElenaPoint.position);
            Globals.ProgressionManager.GetIcon().SetEnabled(false);
            Globals.CinemachineManager.SetPausedState(true);
            Globals.CinemachineManager.SetInputEnabled(false);
            if (soundEffectStart.Length > 0)
                FMODUnity.RuntimeManager.PlayOneShot(soundEffectStart);
            if (actorIdleSound != null) actorIdleSound.mute = true;
            data.conversationStart.Invoke();
            return new KeyValuePair<Vector3, Quaternion>(data.fakeElenaPoint.position, data.fakeElenaPoint.rotation);
        }
        return null;
    }

    public void ConversationEnd(string conversationName, GameObject conversationPlayer) {
        print("Conversation with " + actorName + " ended" + (conversationName.Length > 0 ? ": " + conversationName : "."));
        //fakeElena.SetActive(false);

        DialogueData data = GetDialogueDataFromConversation(conversationName);
        if (data != null) {
            data.cam.Priority = 0;
            Utils.DelayedAction(1.0f, () => //Wait for cameras to blend. 1.0f is default camera blend time
            {
                Globals.CinemachineManager.SetPausedState(false);
                Globals.CinemachineManager.SetInputEnabled(true);
            });

            Globals.Player.VisualEffects.SetLookAt(null);
            if (data.moveCharacterAfterConversation) {
                transform.SetPositionAndRotation(data.newCharacterPosition);
            }

            if (data.moveQuestIconAfterConversation) {
                Vector3 pos = data.questIconPosition == null
                    ? transform.position + data.questIconOffset
                    : data.questIconPosition.position + data.questIconOffset;
                Globals.ProgressionManager.GetIcon().SetPosition(pos);
            }
            Globals.ProgressionManager.GetIcon().SetEnabled(true);
            if (soundEffectEnd.Length > 0)
                FMODUnity.RuntimeManager.PlayOneShot(soundEffectEnd);
            if (actorIdleSound != null) actorIdleSound.mute = false;

            if (data.disableActorAfterDialogue) {
                GetComponent<Usable>().enabled = false;
            }
            data.conversationEnd.Invoke();
        }
    }

    public void ConversationLine(string conversationName, string line, GameObject conversationPlayer) {
    }

    public string GetActorName() {
        return actorName;
    }
}