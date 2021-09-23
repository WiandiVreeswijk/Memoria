using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using static DG.Tweening.DOTween;

public class NotificationManager : MonoBehaviour {
    [SerializeField] private GameObject notificationPrefab;
    [SerializeField] private GameObject notificationsParent;

    private int notificationIndex = 0;
    private const float NOTIFICATION_SPACING = 10f;

    public void NotifyPlayer(string message, float delay = 0.0f) {
        GameObject obj = Instantiate(notificationPrefab, notificationsParent.transform);
        obj.GetComponentInChildren<TextMeshProUGUI>().text = message;
       
        RectTransform rectTransform = obj.GetComponent<RectTransform>();
        obj.transform.position = new Vector3(obj.transform.position.x, rectTransform.rect.height + NOTIFICATION_SPACING + (rectTransform.rect.height + NOTIFICATION_SPACING) * notificationIndex, 0.0f);
       
        CanvasGroup group = obj.GetComponent<CanvasGroup>();
        group.alpha = 0.0f;

        var sequence = Sequence()
            .AppendInterval(delay)
            .AppendCallback(() => { notificationIndex++; })
            .AppendCallback(() => { FMODUnity.RuntimeManager.PlayOneShot("event:/SFXUI/NotificationPopUp"); })
            .Append(To(() => group.alpha, x => group.alpha = x, 1.0f, 1.5f))
            .AppendInterval(5.0f)
            .Append(To(() => group.alpha, x => group.alpha = x, 0.0f, 1.5f))
            .AppendCallback(() => {
                Destroy(obj);
                notificationIndex--;
            });
    }
}
