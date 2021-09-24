using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;

[RequireComponent(typeof(OverrideActorName))]
public class LocalizeActorName : MonoBehaviour
{
    private IEnumerator Start()
    {
        yield return null; // Wait 1 frame for Lua to initialize.
        var overrideActorName = GetComponent<OverrideActorName>();
        var initialName = string.IsNullOrEmpty(overrideActorName.overrideName) ? gameObject.name : overrideActorName.overrideName;
        overrideActorName.overrideName = DialogueLua.GetLocalizedActorField(initialName, "Name").AsString;
        if (string.IsNullOrEmpty(overrideActorName.internalName))
        {
            overrideActorName.internalName = initialName;
        }
    }
}
