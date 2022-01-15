using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace EverNetDemo
{
    public class Popup : MonoBehaviour
    {

        public Text textHeader;
        public Text textMessage;
        public Text textBtnAccept;
        public Text textBtnDenied;
        public Button btnAccept;
        public Button btnDenied;

        private UnityAction previousActionAccept;
        private UnityAction previousActionDenied;

        private void Awake()
        {
            btnAccept.onClick.AddListener(() =>
            {
                this.gameObject.SetActive(false);
            });

            btnDenied.onClick.AddListener(() =>
            {
                this.gameObject.SetActive(false);
            });
        }

        public bool ShowPopup(string header, string message,
            string btnAcceptText, string btnDeniedText,
            UnityAction OnAccept, UnityAction OnDenied = null)
        {
            if (previousActionAccept != null)
            {
                btnAccept.onClick.RemoveListener(previousActionAccept);
            }

            if (previousActionDenied != null)
            {
                btnDenied.onClick.RemoveListener(previousActionDenied);
            }

            if (OnAccept == null)
            {
                Debug.LogError("OnAccept can't null");
                return false;
            }

            this.gameObject.SetActive(true);

            textHeader.text = header;
            textMessage.text = message;
            textBtnAccept.text = btnAcceptText;
            textBtnDenied.text = btnDeniedText;

            btnAccept.gameObject.SetActive(true);
            btnAccept.onClick.AddListener(OnAccept);
            previousActionAccept = OnAccept;

            btnDenied.gameObject.SetActive(OnDenied != null);
            btnDenied.onClick.AddListener(OnDenied);
            previousActionDenied = OnDenied;

            return true;
        }
    }
}

