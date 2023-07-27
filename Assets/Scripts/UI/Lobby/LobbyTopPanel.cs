using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.Serialization;

public class LobbyTopPanel : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TMP_Text _connectionStatusText;

        private readonly string _connectionStatusMessage = "Connection Status: ";

        public void Update()
        {
            _connectionStatusText.text = _connectionStatusMessage + PhotonNetwork.NetworkClientState;
        }
    }