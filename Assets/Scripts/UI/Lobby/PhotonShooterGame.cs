using UnityEngine;

public class PhotonShooterGame
{
    public const string PLAYER_READY = "IsPlayerReady";
    public const string PLAYER_LOADED_LEVEL = "PlayerLoadedLevel";

    public static Color GetColor(int colorChoice)
    {
        switch (colorChoice)
        {
            case 1: return Color.red;
            case 2: return Color.green;
            case 3: return Color.blue;
            case 4: return Color.yellow;
            case 5: return Color.cyan;
            case 6: return Color.grey;
            case 7: return Color.magenta;
            case 8: return Color.white;
        }

        return Color.black;
    }
}