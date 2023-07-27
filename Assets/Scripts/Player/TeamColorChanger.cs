using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;

public class TeamColorChanger : MonoBehaviour
{
    private void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        PhotonView photonView = GetComponentInParent<PhotonView>();

        renderer.material.color = PhotonShooterGame.GetColor(photonView.Owner.GetPlayerNumber());
    }
}