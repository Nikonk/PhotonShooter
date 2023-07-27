using System.IO;

public class PlayerSpawner : Spawner
{
    private readonly string _prefabPath = $"Prefabs{Path.DirectorySeparatorChar}Player{Path.DirectorySeparatorChar}";
    
    private void Awake()
    {
        Spawn(_prefabPath);
    }
}