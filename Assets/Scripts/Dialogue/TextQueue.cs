using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextQueue : MonoBehaviour
{
    public static TextQueue Instance;
    public GameObject cameraCanvas, slideTextPrefab;
    private List<Notification> notifications = new List<Notification>();
    private GameObject activeNotice;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    private void Update()
    {
        if (notifications.Count > 0 && activeNotice == null)
        {
            PlayNotification(notifications[0]);
        }
    }
    private void PlayNotification(Notification notice)
    {
        notifications.RemoveAt(0);
        activeNotice = Instantiate(slideTextPrefab, Vector3.zero, Quaternion.identity, cameraCanvas.transform);
        activeNotice.GetComponent<SetSlideTexts>().SetAll(notice.GetTitle(), notice.GetSubtitle(), notice.GetTitleColor(), notice.GetSubtitleColor());
        Destroy(activeNotice, 2.2f);
    }
    public void AddNotificationToList(Notification notice)
    {
        notifications.Add(notice);
    }
    public void ClearPendingNotifications()
    {
        notifications.Clear();
    }
    public class Notification 
    {
        private string title;
        private string subtitle;
        private Color titleColor;
        private Color subtitleColor;

        public string GetTitle()
        {
            return title;
        }

        public string GetSubtitle()
        {
            return subtitle;
        }

        public Color GetTitleColor()
        {
            return titleColor;
        }

        public Color GetSubtitleColor()
        {
            return subtitleColor;
        }

        public Notification(string t, string s, Color tC, Color sC)
        {
            title = t;
            subtitle = s;
            titleColor = tC;
            subtitleColor = sC;
        }

        public Notification(string t, string s)
        {
            title = t;
            subtitle = s;
            titleColor = Color.white;
            subtitleColor = Color.white;
        }

        public Notification(string t)
        {
            title = t;
            subtitle = string.Empty;
            titleColor = Color.white;
            subtitleColor = Color.white;
        }
    }
    
}
