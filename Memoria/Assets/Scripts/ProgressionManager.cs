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
            .Append("The pocket watch will appear in your screen when Elena is close to an object with a strong ")
            .Color(new Color(1f, 0.843137f, 0f))
            .Append("memory").CloseColor().Append(" attached to it!\n\nHold ")
            .Color(new Color(0f, 1.720795f, 1.027072f, 1f)).Append("spacebar")
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

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            RichTextFormatter formatter = new RichTextFormatter().Size(50).Append("Thank you for playing the demo of ")
                .Color(new Color(0.89f, 0.24f, 0.24f)).Append("MEMORIA").CloseColor().Append("!").Size(30).Append(
                    "\n\nWe greatly appreciate the time and effort you put into playing our game and we can't wait to hear your feedback!\n Please head over to our Discord or website to let us know what you think!\n\n\n")
                .Size(50).Append("Cheers!\n\nthe ").Color(new Color(0.84f, 0f, 0f)).Append("WAEM Game Studios")
                .CloseColor().Append(" team");
            //Globals.Player.PlayerMovementAdventure.Teleport(FindObjectOfType<ReturnPoint>().transform.position);
            Globals.UIManager.NotificationManager.NotifyPlayerBig(formatter, () => { });
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
        Globals.UIManager.NotificationManager.blockNotifications = true;
        Globals.Player.transform.position = FindObjectOfType<ReturnPoint>().transform.position;
        Utils.DelayedAction(5.0f, () => {
            RichTextFormatter formatter = new RichTextFormatter().Size(50).Append("Thank you for playing the demo of ")
                .Color(new Color(0.89f, 0.24f, 0.24f)).Append("MEMORIA").CloseColor().Append("!").Size(30).Append(
                    "\n\nWe greatly appreciate the time and effort you put into playing our game and we can't wait to hear your feedback!\n Please head over to our Discord or website to let us know what you think!\n\n\n")
                .Size(50).Append("Cheers!\n\nthe ").Color(new Color(0.84f, 0f, 0f)).Append("WAEM Game Studios")
                .CloseColor().Append(" team");
            //Globals.Player.PlayerMovementAdventure.Teleport(FindObjectOfType<ReturnPoint>().transform.position);
            Globals.UIManager.NotificationManager.NotifyPlayerBig(formatter, () => { });
        });
    }

}