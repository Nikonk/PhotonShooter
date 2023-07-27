using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using UnityEngine.Serialization;

public class RoomListEntry : MonoBehaviour
{
    [SerializeField] private TMP_Text _roomNameText;
    [SerializeField] private TMP_Text _roomPlayersText;
    [SerializeField] private Button _joinRoomButton;

    private string _roomName;

    public void Start()
    {
        _joinRoomButton.onClick.AddListener(() =>
        {
            if (PhotonNetwork.InLobby)
                PhotonNetwork.LeaveLobby();

            PhotonNetwork.JoinRoom(_roomName);
        });
    }

    public void Initialize(string name, int currentPlayers, int maxPlayers)
    {
        _roomName = name;

        _roomNameText.text = name;
        _roomPlayersText.text = currentPlayers + " / " + maxPlayers;
    }
}