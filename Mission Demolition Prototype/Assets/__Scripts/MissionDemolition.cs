using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum GameMode
{
    idle,
    playing,
    levelEnd
}

public class MissionDemolition : MonoBehaviour
{
    static private MissionDemolition S;
    static public GameObject uitRestartButton;


    [Header("Set In Inspector")]
    public Text uitLevel;
    public Text uitShots;
    public Text uitButton;
    public GameObject prefabButton;
    public Vector3 castlePos;
    public GameObject[] castles;

    [Header("Set Dynamically")]
    public int level; // current level
    public int levelMax; //count levels
    public int shotsTaken;
    public GameObject castle;
    public GameMode mode = GameMode.idle;
    public string showing = "Show Slingshot"; //FollowCam

    private void Start()
    {
        S = this;

        level = 0;
        levelMax = castles.Length;
        StartLevel();
    }

    private void StartLevel()
    {
        if (castle != null)
        {
            Destroy(castle);
        }

        GameObject[] gos = GameObject.FindGameObjectsWithTag("Projectile");
        foreach (GameObject pTemp in gos)
        {
            Destroy(pTemp);
        }

        //Create new castle
        castle = Instantiate(castles[level]);
        castle.transform.position = castlePos;
        shotsTaken = 0;

        //reset camera in start pos
        SwitchView("Show Both");
        ProjectileLine.S.Clear();

        //reset target
        Goal.goalMet = false;

        UpdateGUI();

        mode = GameMode.playing;
    }

    private void UpdateGUI()
    {

        uitLevel.text = $"Level: {level + 1} of {+levelMax}";
        uitShots.text = $"Shots Taken: {shotsTaken}";
    }

    private void Update()
    {
        UpdateGUI();

        //Check ending level
        if ((mode == GameMode.playing) && Goal.goalMet)
        {
            mode = GameMode.levelEnd;
            SwitchView("ShowBoth");
            Invoke("NextLevel", 2f);
        }

        //show restart button
        if (S.shotsTaken >= 5)
        {
            prefabButton.SetActive(true);
        }
    }

    private void NextLevel()
    {
        level++;
        if (level == levelMax)
        {
            level = 0;
        }

        StartLevel();
    }

    public void SwitchView(string eView = "")
    {
        if (eView == "")
        {
            eView = uitButton.text;
        }

        showing = eView;
        switch (showing)
        {
            case "Show Slingshot":
                FollowCam.POI = null;
                uitButton.text = "Show Castle";
                break;

            case "Show Castle":
                FollowCam.POI = S.castle;
                uitButton.text = "Show Both";
                break;

            case "Show Both":
                FollowCam.POI = GameObject.Find("ViewBoth");
                uitButton.text = "Show Slingshot";
                break;
        }
    }

    public static void ShotFired()
    {
        S.shotsTaken++;
    }
}
