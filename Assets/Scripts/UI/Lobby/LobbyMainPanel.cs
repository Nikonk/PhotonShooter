using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using TMPro;

using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine.Serialization;

public class LobbyMainPanel : MonoBehaviourPunCallbacks
{
    [Header("Login Panel")]
    [SerializeField] private GameObject _loginPanel;
    [SerializeField] private TMP_InputField _playerNameInput;

    [Header("Selection Panel")]
    [SerializeField] private GameObject _selectionPanel;

    [Header("Create Room Panel")]
    [SerializeField] private GameObject _createRoomPanel;
    [SerializeField] private TMP_InputField _roomNameInputField;
    [SerializeField] private TMP_InputField _maxPlayersInputField;

    [Header("Join Random Room Panel")]
    [SerializeField] private GameObject _joinRandomRoomPanel;

    [Header("Room List Panel")]
    [SerializeField] private GameObject _roomListPanel;
    [SerializeField] private GameObject _roomListContent;
    [SerializeField] private GameObject _roomListEntryPrefab;

    [Header("Inside Room Panel")]
    [SerializeField] private GameObject _insideRoomPanel;
    [SerializeField] private Button _startGameButton;
    [SerializeField] private GameObject _playerListEntryPrefab;

    private Dictionary<string, RoomInfo> _cachedRoomList;
    private Dictionary<string, GameObject> _roomListEntries;
    private Dictionary<int, GameObject> _playerListEntries;

    public void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        _cachedRoomList = new Dictionary<string, RoomInfo>();
        _roomListEntries = new Dictionary<string, GameObject>();

        _playerNameInput.text = "Player " + Random.Range(1000, 10000);
    }

    public override void OnConnectedToMaster()
    {
        this.SetActivePanel(_selectionPanel.name);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        ClearRoomListView();

        UpdateCachedRoomList(roomList);
        UpdateRoomListView();
    }

    public override void OnJoinedLobby()
    {
        _cachedRoomList.Clear();
        ClearRoomListView();
    }

    public override void OnLeftLobby()
    {
        _cachedRoomList.Clear();
        ClearRoomListView();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        SetActivePanel(_selectionPanel.name);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        SetActivePanel(_selectionPanel.name);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        string roomName = "Room " + Random.Range(1000, 10000);

        RoomOptions options = new RoomOptions { MaxPlayers = 8 };

        PhotonNetwork.CreateRoom(roomName, options, null);
    }

    public override void OnJoinedRoom()
    {
        _cachedRoomList.Clear();

        SetActivePanel(_insideRoomPanel.name);

        _playerListEntries ??= new Dictionary<int, GameObject>();

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            GameObject entry = Instantiate(_playerListEntryPrefab, _insideRoomPanel.transform, true);
            entry.transform.localScale = Vector3.one;
            entry.GetComponent<PlayerListEntry>().Initialize(player.ActorNumber, player.NickName);

            if (player.CustomProperties.TryGetValue(PhotonShooterGame.PLAYER_READY, out object isPlayerReady))
                entry.GetComponent<PlayerListEntry>().SetPlayerReady((bool)isPlayerReady);

            _playerListEntries.Add(player.ActorNumber, entry);
        }

        _startGameButton.gameObject.SetActive(CheckPlayersReady());

        Hashtable props = new Hashtable
        {
            { PhotonShooterGame.PLAYER_LOADED_LEVEL, false }
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        PhotonNetwork.LocalPlayer.SetPlayerNumber(PhotonNetwork.LocalPlayer.ActorNumber);
    }

    public override void OnLeftRoom()
    {
        SetActivePanel(_selectionPanel.name);

        foreach (GameObject entry in _playerListEntries.Values)
            Destroy(entry.gameObject);

        _playerListEntries.Clear();
        _playerListEntries = null;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        GameObject entry = Instantiate(_playerListEntryPrefab, _insideRoomPanel.transform, true);
        entry.transform.localScale = Vector3.one;
        entry.GetComponent<PlayerListEntry>().Initialize(newPlayer.ActorNumber, newPlayer.NickName);

        _playerListEntries.Add(newPlayer.ActorNumber, entry);

        _startGameButton.gameObject.SetActive(CheckPlayersReady());
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Destroy(_playerListEntries[otherPlayer.ActorNumber].gameObject);
        _playerListEntries.Remove(otherPlayer.ActorNumber);

        _startGameButton.gameObject.SetActive(CheckPlayersReady());
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber)
            _startGameButton.gameObject.SetActive(CheckPlayersReady());
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        _playerListEntries ??= new Dictionary<int, GameObject>();

        if (_playerListEntries.TryGetValue(targetPlayer.ActorNumber, out GameObject entry))
            if (changedProps.TryGetValue(PhotonShooterGame.PLAYER_READY, out object isPlayerReady))
                entry.GetComponent<PlayerListEntry>().SetPlayerReady((bool)isPlayerReady);

        _startGameButton.gameObject.SetActive(CheckPlayersReady());
    }

    public void OnBackButtonClicked()
    {
        if (PhotonNetwork.InLobby)
            PhotonNetwork.LeaveLobby();

        SetActivePanel(_selectionPanel.name);
    }

    public void OnCreateRoomButtonClicked()
    {
        string roomName = _roomNameInputField.text;
        roomName = (roomName.Equals(string.Empty)) ? "Room " + Random.Range(1000, 10000) : roomName;

        int.TryParse(_maxPlayersInputField.text, out int maxPlayers);
        maxPlayers = Mathf.Clamp(maxPlayers, 2, 8);

        RoomOptions options = new RoomOptions { MaxPlayers = maxPlayers, PlayerTtl = 10000 };

        PhotonNetwork.CreateRoom(roomName, options, null);
    }

    public void OnJoinRandomRoomButtonClicked()
    {
        SetActivePanel(_joinRandomRoomPanel.name);

        PhotonNetwork.JoinRandomRoom();
    }

    public void OnLeaveGameButtonClicked()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void OnLoginButtonClicked()
    {
        string playerName = _playerNameInput.text;

        if (!playerName.Equals(""))
        {
            PhotonNetwork.LocalPlayer.NickName = playerName;
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            Debug.LogError("Player Name is invalid.");
        }
    }

    public void OnRoomListButtonClicked()
    {
        if (!PhotonNetwork.InLobby)
            PhotonNetwork.JoinLobby();

        SetActivePanel(_roomListPanel.name);
    }

    public void OnStartGameButtonClicked()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;

        PhotonNetwork.LoadLevel("Game");
    }

    private bool CheckPlayersReady()
    {
        if (!PhotonNetwork.IsMasterClient)
            return false;

        foreach (Player player in PhotonNetwork.PlayerList)
            if (player.CustomProperties.TryGetValue(PhotonShooterGame.PLAYER_READY, out object isPlayerReady))
            {
                if (!(bool)isPlayerReady)
                    return false;
            }
            else
            {
                return false;
            }

        return true;
    }

    private void ClearRoomListView()
    {
        foreach (GameObject entry in _roomListEntries.Values)
            Destroy(entry.gameObject);

        _roomListEntries.Clear();
    }

    public void LocalPlayerPropertiesUpdated()
    {
        _startGameButton.gameObject.SetActive(CheckPlayersReady());
    }

    private void SetActivePanel(string activePanel)
    {
        _loginPanel.SetActive(activePanel.Equals(_loginPanel.name));
        _selectionPanel.SetActive(activePanel.Equals(_selectionPanel.name));
        _createRoomPanel.SetActive(activePanel.Equals(_createRoomPanel.name));
        _joinRandomRoomPanel.SetActive(activePanel.Equals(_joinRandomRoomPanel.name));
        _roomListPanel.SetActive(activePanel.Equals(_roomListPanel.name));
        _insideRoomPanel.SetActive(activePanel.Equals(_insideRoomPanel.name));
    }

    private void UpdateCachedRoomList(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            if (!info.IsOpen || !info.IsVisible || info.RemovedFromList)
            {
                if (_cachedRoomList.ContainsKey(info.Name))
                    _cachedRoomList.Remove(info.Name);

                continue;
            }

            if (_cachedRoomList.ContainsKey(info.Name))
                _cachedRoomList[info.Name] = info;
            else
                _cachedRoomList.Add(info.Name, info);
        }
    }

    private void UpdateRoomListView()
    {
        foreach (RoomInfo info in _cachedRoomList.Values)
        {
            GameObject entry = Instantiate(_roomListEntryPrefab, _roomListContent.transform, true);
            entry.transform.localScale = Vector3.one;
            entry.GetComponent<RoomListEntry>().Initialize(info.Name, info.PlayerCount, info.MaxPlayers);

            _roomListEntries.Add(info.Name, entry);
        }
    }
}