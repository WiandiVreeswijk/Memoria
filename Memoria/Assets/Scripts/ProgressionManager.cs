using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;

public class ProgressionManager : MonoBehaviour {
    private Icon questIcon;
    private bool watchCollected = false;

    public void CollectWatch() {
        watchCollected = true;
        Globals.MemoryWatchManager.EnableMemoryWatch();
        RichTextFormatter formatter = new RichTextFormatter();


        formatter.Size(50).Append("You have found grandma's pocket watch!\n\n").Size(40)
            .Append("The pocket watch will appear when Elena is close to an object with a strong ")
            .Color(new Color(255f / 255f, 215f / 255f, 0f))
            .Append("memory").CloseColor().Append(" attached to it!\n\nHold ")
            .Color(new Color(0f, 96f / 255f, 100f / 255)).Append("spacebar")
            .CloseColor().Append(" to activate the pocket watch when you are near such an object");
        Globals.UIManager.NotificationManager.NotifyPlayerBig(formatter, () => { });
    }

    //For debugger
    public void CollectWatchManual() {
        watchCollected = true;
        Globals.MemoryWatchManager.EnableMemoryWatch();
    }

    public Icon GetIcon() {
        return questIcon;
    }

    public void OnGlobalsInitializeType(Globals.GlobalsType previousGlobalsType, Globals.GlobalsType currentGlobalsType) {
        if (currentGlobalsType == Globals.GlobalsType.NEIGHBORHOOD) { //#TODO: This is temporary
            if (previousGlobalsType == Globals.GlobalsType.OBLIVION) {
                InitializeProgressionBackFromChasing();
            } else {
                GameObject manfred = Utils.FindUniqueObject<Manfred>().gameObject;
                questIcon = Globals.IconManager.AddWorldIcon("oma",
                    manfred.transform.position + new Vector3(0, 2.25f, 0));
            }
        } else {
            if (questIcon != null) questIcon.SetEnabled(false);
        }
    }

    public void InitializeProgressionBackFromChasing() {
        DialogueHandler[] dialogueHandlers = GameObject.FindObjectsOfType<DialogueHandler>();
        DialogueHandler oma = dialogueHandlers.First(x => x.actorName == "oma");
        DialogueHandler hanna = dialogueHandlers.First(x => x.actorName == "hanna");
        DialogueHandler manfred = dialogueHandlers.First(x => x.actorName == "manfred");
        oma.SetDialogueEnabled(false);
        hanna.SetDialogueEnabled(false);
        manfred.SetDialogueEnabled(false);
        CollectWatchManual();
        Globals.MenuController.CloseMenu(0.0f);
        FindObjectOfType<OmaHouseDoor>().OpenDoor();
        FindObjectOfType<WijkOpeningCutscene>().Skip();
        Globals.CinemachineManager.ClearNextBlend();
        Globals.Player.transform.position = FindObjectOfType<ReturnPoint>().transform.position;
        Utils.DelayedAction(5.0f, () => {
        //Globals.Player.PlayerMovementAdventure.Teleport(FindObjectOfType<ReturnPoint>().transform.position);
            Globals.UIManager.NotificationManager.NotifyPlayerBig("Fiets", () => { });
        });
    }

}