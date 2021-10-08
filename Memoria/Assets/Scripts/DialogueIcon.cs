using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueIcon : MonoBehaviour {
    private GameObject dialogueIcon;
    public float heightOffset = 1.75f;
    void Start() {
        dialogueIcon = Instantiate(Globals.IconManager.dialogueIconPrefab, transform);
        dialogueIcon.transform.localScale = new Vector3(0.65f, 0.65f, 0.65f);
    }

    void Update() {
        if (dialogueIcon.activeInHierarchy) {
            dialogueIcon.transform.position = transform.position + new Vector3(0f, Mathf.Sin(Time.time / 2f) / 20f + heightOffset, 0f);
            dialogueIcon.transform.Rotate(new Vector3(0f, Time.deltaTime * 10f, 0f));
        }
    }

    public void SetEnabled(bool toggle) {
        dialogueIcon.SetActive(toggle);
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position + new Vector3(0f, heightOffset, 0f), 0.025f);
        Gizmos.DrawLine(transform.position + new Vector3(0f, heightOffset + 1f / 20f, 0f),
            transform.position + new Vector3(0f, heightOffset - 1f / 20f, 0f));
    }
}
