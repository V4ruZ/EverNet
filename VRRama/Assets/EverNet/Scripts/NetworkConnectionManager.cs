using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace EverNet
{
    public class NetworkConnectionManager : MonoBehaviourPunCallbacks
    {

        private enum NetworkType
        {
            PhotonNet,
            NodeJSNet
        }

        [SerializeField]
        private NetworkType usingNetType;

        private static NetworkConnectionManager localNetConnection;

        public delegate void DelegateHandle();
        public delegate void DelegateHandleReturnPlayer(string playerID);

        public DelegateHandle CallbackOnConnected;
        public DelegateHandle CallbackOnDisconnected;
        public DelegateHandle CallbackOnJoinedLobby;
        public DelegateHandle CallbackOnLeftLobby;
        public DelegateHandle CallbackOnJoinedRoom;
        public DelegateHandle CallbackOnJoinRoomFailed;
        public DelegateHandle CallbackOnLeftRoom;
        public DelegateHandleReturnPlayer CallbackOnPlayerEnteredRoom;
        public DelegateHandleReturnPlayer CallbackOnPlayerLeftRoom;

        public static NetworkConnectionManager Instance
        {
            get
            {
                if (localNetConnection == null)
                {
                    NetworkConnectionManager[] netManager = GameObject.FindObjectsOfType<NetworkConnectionManager>();
                    if (netManager.Length == 0)
                    {
                        Debug.LogError("EverNet : Not found [NetConnectionManager] check on your scene again.");
                        return null;
                    }
                    else if (netManager.Length > 1)
                    {
                        Debug.LogError("EverNet : [NetConnectionManager] should have only one on your scene.");
                        return null;
                    }
                    else
                    {
                        localNetConnection = netManager[0];
                    }
                }

                return localNetConnection;
            }
        }

        public void ConnectToServer()
        {
            switch(usingNetType)
            {
                case NetworkType.PhotonNet:
                {
                    PhotonNetwork.ConnectUsingSettings();
                    break;
                }
                case NetworkType.NodeJSNet:
                {
                    //TODO : Open callback and connecting to server here.
                    break;
                }
            }
        }

        public void Disconnect()
        {
             switch(usingNetType)
            {
                case NetworkType.PhotonNet:
                {
                    PhotonNetwork.Disconnect();
                    break;
                }
                case NetworkType.NodeJSNet:
                {
                    //TODO : Call disconnect here.
                    break;
                }
            }
        }

        public void JoinLobby()
        {
            switch(usingNetType)
            {
                case NetworkType.PhotonNet:
                {
                    PhotonNetwork.JoinLobby();
                    break;
                }
                case NetworkType.NodeJSNet:
                {
                    //TODO : Call join lobby here.
                    break;
                }
            }
        }

        public void CreateRoom(string roomName, bool bIsOpen, bool bIsVisible, int maxPlayer)
        {
            switch(usingNetType)
            {
                case NetworkType.PhotonNet:
                {
                    RoomOptions roomOptions = new RoomOptions();
                    roomOptions.IsOpen = bIsOpen;
                    roomOptions.IsVisible = bIsVisible;
                    roomOptions.MaxPlayers = (byte)maxPlayer;
                    PhotonNetwork.CreateRoom(roomName, roomOptions);
                    break;
                }
                case NetworkType.NodeJSNet:
                {
                    //TODO : Call create room here.
                    break;
                }
            }
        }

        public void JoinRoom(string roomName)
        {
            switch(usingNetType)
            {
                case NetworkType.PhotonNet:
                {
                    PhotonNetwork.JoinRoom(roomName);
                    break;
                }
                case NetworkType.NodeJSNet:
                {
                    //TODO : Call join room here.
                    break;
                }
            }
        }

        public void LeaveRoom()
        {
            switch(usingNetType)
            {
                case NetworkType.PhotonNet:
                {
                    if(PhotonNetwork.InRoom)
                    {
                        PhotonNetwork.LeaveRoom();
                    }
                    else
                    {
                        Debug.LogWarning("EverNet : [PhotonNet] You are not in room.");
                    }
                    break;
                }
                case NetworkType.NodeJSNet:
                {
                    //TODO : Call leave room here.
                    break;
                }
            }
        }

        #region PhotonNetworkCallback
        public override void OnConnected()
        {
            /*base.OnConnected();

            if(CallbackOnConnected != null)
            {
                CallbackOnConnected();
            }*/
        }

        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();

            if (CallbackOnConnected != null)
            {
                CallbackOnConnected();
            }
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            base.OnDisconnected(cause);

            if(CallbackOnDisconnected != null)
            {
                CallbackOnDisconnected();
            }
        }

        public override void OnJoinedLobby()
        {
            base.OnJoinedLobby();

            if(CallbackOnJoinedLobby != null)
            {
                CallbackOnJoinedLobby();
            }
        }

        public override void OnLeftLobby()
        {
            base.OnLeftLobby();

            if(CallbackOnLeftLobby != null)
            {
                CallbackOnLeftLobby();
            }
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();

            if(CallbackOnJoinedRoom != null)
            {
                CallbackOnJoinedRoom();
            }
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            base.OnJoinRoomFailed(returnCode, message);

            if(CallbackOnJoinRoomFailed != null)
            {
                CallbackOnJoinRoomFailed();
            }
        }

        public override void OnLeftRoom()
        {
            base.OnLeftRoom();

            if(CallbackOnLeftRoom != null)
            {
                CallbackOnLeftRoom();
            }
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            base.OnPlayerEnteredRoom(newPlayer);

            if(CallbackOnPlayerEnteredRoom != null)
            {
                CallbackOnPlayerEnteredRoom(newPlayer.UserId);
            }
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            base.OnPlayerLeftRoom(otherPlayer);

            if(CallbackOnPlayerLeftRoom != null)
            {
                CallbackOnPlayerLeftRoom(otherPlayer.UserId);
            }
        }
        #endregion
    }

}
