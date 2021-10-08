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

        string localized = Globals.Localization.Get("INSTR_WATCH");
        Globals.UIManager.NotificationManager.NotifyPlayerBig(localized, () => { });
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
                    manfred.transform.position + new Vector3(0, 2.75f, 0));
            }
        }

        if (questIcon != null)
        {
             questIcon.SetEnabled(FindObjectOfType<WijkOpeningCutscene>().ShouldSkip());
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
        Globals.UIManager.NotificationManager.SetBlockNotifications();
        Globals.Player.transform.position = FindObjectOfType<ReturnPoint>().transform.position;
        Utils.DelayedAction(5.0f, () => {
            string localized = Globals.Localization.Get("DEMO_FINISHED");
            print("demoFinish");
            Globals.UIManager.NotificationManager.NotifyPlayerBig(localized, () => { });
        });
    }
}