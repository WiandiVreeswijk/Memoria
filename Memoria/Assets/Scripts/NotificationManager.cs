using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using static DG.Tweening.DOTween;

public class NotificationManager : MonoBehaviour {
    [SerializeField] private GameObject notificationPrefab;
    [SerializeField] private GameObject notificationsParent;
    [SerializeField] private NotificationPanel notificationPanel;

    private bool blockNotifications = false;
    private List<Notification> cachedNotifications = new List<Notification>();

    private int notificationIndex = 0;
    private const float NOTIFICATION_SPACING = 10f;

    public delegate void OnAcceptNotification();
    private OnAcceptNotification currentOnAcceptNotification;
    public void NotifyPlayerBig(string message, OnAcceptNotification onAcceptNotification) {
        currentOnAcceptNotification = onAcceptNotification;
        notificationPanel.Open(message);
    }

    public void NotifyPlayerBig(RichTextFormatter formatter, OnAcceptNotification onAcceptNotification) {
        NotifyPlayerBig(formatter.Finalize(), onAcceptNotification);
    }

    public void SetBlockNotifications() {
        blockNotifications = true;
    }

    //private void Update() {
    //    if (Input.GetKeyDown(KeyCode.B)) {
    //        RichTextFormatter formatter = new RichTextFormatter();
    //
    //        formatter.Color(Color.red).Size(50).Append("Dit is een test!\n").CloseSize().Append("Dit ook!\n")
    //            .Font("Mouse SDF").Color(Color.blue).Append("u")
    //            .CloseFont().CloseColor().Append(" Fiets");
    //
    //        NotifyPlayerBig(formatter, () => print("Pressed!"));
    //    }
    //}

    public void AcceptNotificationButtonClick() {
        if (currentOnAcceptNotification != null) currentOnAcceptNotification();
        currentOnAcceptNotification = null;
        notificationPanel.Close();
    }

    public void NotifyPlayer(string message, float delay = 0.0f) {
        print("blocked: " + blockNotifications);
        if (blockNotifications) {
            print("Notification blocked!");
            return;
        }
        GameObject obj = Instantiate(notificationPrefab, notificationsParent.transform);
        Notification notification = obj.GetComponent<Notification>();
        notification.Set(message);

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

    public List<Notification> GetActiveNotifications() {
        return notificationsParent.GetComponentsInChildren<Notification>().ToList();
    }

    public void CacheActiveNotifications() {
        cachedNotifications = GetActiveNotifications();
        foreach (Notification notification in cachedNotifications) {
            Destroy(notification.gameObject);
        }
    }

    public void ReleaseCachedNotifications() {
        Utils.DelayedAction(2.5f, () => {
            foreach (Notification notification in cachedNotifications) {
                NotifyPlayer(notification.GetText(), 0f);
            }
            cachedNotifications.Clear();
        });
    }
}
