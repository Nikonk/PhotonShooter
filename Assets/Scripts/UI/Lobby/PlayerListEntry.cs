using UnityEngine;
using UnityEngine.UI;

using TMPro;

using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;
using Photon.Pun.UtilityScripts;

public class PlayerListEntry : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text _playerNameText;
    [SerializeField] private Image _playerColorImage;
    [SerializeField] private Button _playerReadyButton;
    [SerializeField] private Image _playerReadyImage;

    private int _ownerId;
    private bool _isPlayerReady;

    public void OnEnable()
    {
        PlayerNumbering.OnPlayerNumberingChanged += OnPlayerNumberingChanged;
    }

    public void Start()
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber != _ownerId)
        {
            _playerReadyButton.gameObject.SetActive(false);
        }
        else
        {
            Hashtable initialProps = new Hashtable() { { PhotonShooterGame.PLAYER_READY, _isPlayerReady } };
            PhotonNetwork.LocalPlayer.SetCustomProperties(initialProps);

            _playerReadyButton.onClick.AddListener(() =>
            {
                _isPlayerReady = !_isPlayerReady;
                SetPlayerReady(_isPlayerReady);

                Hashtable props = new Hashtable() { { PhotonShooterGame.PLAYER_READY, _isPlayerReady } };
                PhotonNetwork.LocalPlayer.SetCustomProperties(props);

                if (PhotonNetwork.IsMasterClient)
                    FindObjectOfType<LobbyMainPanel>().LocalPlayerPropertiesUpdated();
            });
        }
    }

    public void OnDisable()
    {
        PlayerNumbering.OnPlayerNumberingChanged -= OnPlayerNumberingChanged;
    }

    public void Initialize(int playerId, string playerName)
    {
        _ownerId = playerId;
        _playerNameText.text = playerName;
        
        _playerColorImage.color = PhotonShooterGame.GetColor(playerId);
    }

    private void OnPlayerNumberingChanged()
    {
        foreach (Player p in PhotonNetwork.PlayerList)
            if (p.ActorNumber == _ownerId)
                _playerColorImage.color = PhotonShooterGame.GetColor(p.GetPlayerNumber());
    }

    public void SetPlayerReady(bool playerReady)
    {
        _playerReadyButton.GetComponentInChildren<TMP_Text>().text = playerReady ? "Ready!" : "Ready?";
        _playerReadyImage.enabled = playerReady;
    }
}