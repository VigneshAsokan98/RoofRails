using System;
using System.IO;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader Instance { get; private set; }

    [SerializeField]
    TextAsset[] Levels;
    Vector3 SpawnPosition;

    int CurrentLevel;
    float interval = 2.6f;

    [SerializeField]
    GameObject Gems;
    [SerializeField]
    GameObject Ground;
    [SerializeField]
    GameObject Saw;
    [SerializeField]
    GameObject Adder;
    [SerializeField]
    GameObject Enemy;
    [SerializeField]
    GameObject Rail;
    [SerializeField]
    GameObject FireTile;
    [SerializeField]
    GameObject FinishLine;
    [SerializeField]
    GameObject Trampoline;

    GameObject LevelObjects = null;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        InitLevel();
    }

    public void InitLevel()
    {

        PlayerController.instance.ResetPlayer();


        if (LevelObjects != null)
            Destroy(LevelObjects);

        LevelObjects = new GameObject();
        LevelObjects.name = "LevelElements";
        CurrentLevel = PlayerPrefs.GetInt("CurrentLevel", 0);
        string text = Levels[CurrentLevel].text;
        string[] lines = text.Split('\n');

        SpawnPosition = Vector3.zero;
        SpawnPosition.x = 5.2f;
        LoadLevel(lines);
    }

    private void LoadLevel(string[] lines)
    {
        for (int height = 0; height < lines.Length; height++)
        {
            for (int width = 0; width < lines[height].Length; width++)
            {
                LoadPrefab(lines[height][width]);
                SpawnPosition.x -= interval;
            }
            SpawnPosition.x = 5.2f;
            SpawnPosition.z += interval;
        }
    }

    private void LoadPrefab(char v)
    {
        switch(v)
        {
            case '.':
                return;
            case 'p':
                SpawnPosition.y -= interval * 2;
                Instantiate(Ground, SpawnPosition, Quaternion.identity, LevelObjects.transform);
                break;
            case 'g':
                Instantiate(Gems, SpawnPosition, Quaternion.identity, LevelObjects.transform);
                break;
            case 'a':
                Instantiate(Adder, SpawnPosition, Quaternion.identity, LevelObjects.transform);
                break;
            case 'e':
                Instantiate(Enemy, SpawnPosition, Quaternion.identity, LevelObjects.transform);
                break;
            case 's':
                Instantiate(Saw, SpawnPosition, Quaternion.identity, LevelObjects.transform);
                break;
            case 'f':
                Instantiate(FireTile, SpawnPosition, Quaternion.identity, LevelObjects.transform);
                break;
            case 'l':
                Instantiate(FinishLine, SpawnPosition, Quaternion.identity, LevelObjects.transform);
                break;
            case 't':
                Instantiate(Trampoline, SpawnPosition, Quaternion.identity, LevelObjects.transform);
                break;
            case 'r':
                SpawnPosition.y -= interval * 2;
                Instantiate(Rail, SpawnPosition, Quaternion.identity, LevelObjects.transform);
                break;
        }
    }
}
