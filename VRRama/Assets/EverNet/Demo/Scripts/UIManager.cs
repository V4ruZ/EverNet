using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EverNet;
using UnityEngine.UI;

namespace EverNetDemo
{
    public class UIManager : MonoBehaviour
    {
        public enum PanelType
        {
            Connect,
            Lobby,
            Room
        }

        public class Panel
        {
            public GameObject root;

            public virtual void SetVisible(bool bIsVisible)
            {
                root.SetActive(bIsVisible);
            }

            public virtual void BindButtonEvent() { }
        }

        [System.Serializable]
        public class PanelConnect : Panel
        {
            public Button btnConnect;

            public override void BindButtonEvent()
            {
                base.BindButtonEvent();

                btnConnect.onClick.AddListener(() =>
                {
                    NetworkConnectionManager.Instance.ConnectToServer();
                });
            }
        }

        [System.Serializable]
        public class PanelLobby : Panel
        {
            public InputField inputRoomNameCreate;
            public InputField inputRoomNameJoin;

            public GameObject subPanelSelectRole;
            public GameObject subPanelCreateRoom;
            public GameObject subPanelJoinRoom;

            public Button btnGotoCreateRoom;
            public Button btnGotoJoinRoom;
            public Button btnBackToSelectRole;
            public Button btnCreateRoom;
            public Button btnJoinRoom;
            public Button btnDisconnect;

            public override void SetVisible(bool bIsVisible)
            {
                base.SetVisible(bIsVisible);

                inputRoomNameCreate.text = string.Empty;
                inputRoomNameJoin.text = string.Empty;
                btnDisconnect.gameObject.SetActive(true);
                btnBackToSelectRole.gameObject.SetActive(false);
                subPanelSelectRole.SetActive(true);
                subPanelCreateRoom.SetActive(false);
                subPanelJoinRoom.SetActive(false);
            }

            public override void BindButtonEvent()
            {
                base.BindButtonEvent();

                btnGotoCreateRoom.onClick.AddListener(() =>
                {
                    subPanelCreateRoom.SetActive(true);
                    subPanelSelectRole.SetActive(false);

                    btnDisconnect.gameObject.SetActive(false);
                    btnBackToSelectRole.gameObject.SetActive(true);
                });

                btnGotoJoinRoom.onClick.AddListener(() =>
                {
                    subPanelJoinRoom.SetActive(true);
                    subPanelSelectRole.SetActive(false);

                    btnDisconnect.gameObject.SetActive(false);
                    btnBackToSelectRole.gameObject.SetActive(true);
                });

                btnBackToSelectRole.onClick.AddListener(() =>
                {
                    subPanelCreateRoom.SetActive(false);
                    subPanelJoinRoom.SetActive(false);
                    subPanelSelectRole.SetActive(true);

                    btnDisconnect.gameObject.SetActive(true);
                    btnBackToSelectRole.gameObject.SetActive(false);
                });

                btnCreateRoom.onClick.AddListener(() =>
                {
                    NetworkConnectionManager.Instance.CreateRoom(inputRoomNameCreate.text, true, true, 5);
                });

                btnJoinRoom.onClick.AddListener(() =>
                {
                    NetworkConnectionManager.Instance.JoinRoom(inputRoomNameJoin.text);
                });

                btnDisconnect.onClick.AddListener(() =>
                {
                    NetworkConnectionManager.Instance.Disconnect();
                });
            }
        }

        [System.Serializable]
        public class PanelRoom : Panel
        {
            public Button btnLeaveRoom;

            public override void BindButtonEvent()
            {
                base.BindButtonEvent();

                btnLeaveRoom.onClick.AddListener(() =>
                {
                    NetworkConnectionManager.Instance.LeaveRoom();
                });
            }
        }

        public PanelConnect panelConnect;
        public PanelLobby panelLobby;
        public PanelRoom panelRoom;
        public Popup popup;

        private Dictionary<PanelType, Panel> panelList = new Dictionary<PanelType, Panel>();

        private void Start()
        {
            panelList.Add(PanelType.Connect, panelConnect);
            panelList.Add(PanelType.Lobby, panelLobby);
            panelList.Add(PanelType.Room, panelRoom);

            BindNetEvent();
            BindButtonEvent();

            OpenPanel(PanelType.Connect);
        }

        private void BindNetEvent()
        {
            NetworkConnectionManager.Instance.CallbackOnConnected += OnConnected;
            NetworkConnectionManager.Instance.CallbackOnDisconnected += OnDisconnected;
            NetworkConnectionManager.Instance.CallbackOnJoinedLobby += OnJoinedLobby;
            NetworkConnectionManager.Instance.CallbackOnJoinedRoom += OnJoinedRoom;
            NetworkConnectionManager.Instance.CallbackOnJoinRoomFailed += OnJoinedRoomFail;
            NetworkConnectionManager.Instance.CallbackOnLeftRoom += OnLeftRoom;
            NetworkConnectionManager.Instance.CallbackOnPlayerEnteredRoom += OnPlayerEnterRoom;
            NetworkConnectionManager.Instance.CallbackOnPlayerLeftRoom += OnPlayerLeftRoom;
        }

        private void BindButtonEvent()
        {
            foreach(var panel in panelList)
            {
                panel.Value.BindButtonEvent();
            }
        }

        public void OpenPanel(PanelType panelType)
        {
            foreach (var panel in panelList)
            {
                panel.Value.SetVisible(false);
            }

            panelList[panelType].SetVisible(true);
        }


        #region Callback
        public void OnConnected()
        {
            NetworkConnectionManager.Instance.JoinLobby();
        }

        public void OnDisconnected()
        {
            OpenPanel(PanelType.Connect);
        }

        public void OnJoinedLobby()
        {
            OpenPanel(PanelType.Lobby);
        }

        public void OnJoinedRoom()
        {
            OpenPanel(PanelType.Room);
        }

        public void OnJoinedRoomFail()
        {
            popup.ShowPopup("Fail",
                            "Join room fail.",
                            "OK",
                            "",
                            ()=>
                            {
                                Debug.Log("Click ok button");
                            });
        }

        public void OnLeftRoom()
        {
            OpenPanel(PanelType.Lobby);
        }

        public void OnPlayerEnterRoom(string playerID)
        {
            Debug.Log($"Player : {playerID} join in room.");
        }

        public void OnPlayerLeftRoom(string playerID)
        {
            Debug.Log($"Player : {playerID} left from room.");
        }
        #endregion
    }
}
