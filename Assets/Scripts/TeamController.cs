using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Team
{
    public int wins;
    public int losses;
    public string teamName;

}

public class Week
{
    public Team homeTeam;
    public Team awayTeam;
    public int homeScore;
    public int awayScore;
    public bool playerAway;

}

public class TeamController : MonoBehaviour {
    //This class' job will hold the player's team and other various teams
    //It will also handle schedules and win/loss ratios

    public int weekCount = 1;
    public Team playerTeam = new Team();
    public Team[] teams = new Team[scheduleLength];
    private int playerBigWins = 0;
    public string[] randomNames;
    public Week[] schedule = new Week[scheduleLength];

    //How many games should the player play in one season?
    public static int scheduleLength = 5;

    public GameObject WeekUI = null;
    public Canvas scheduleCanvas = null;


    private GameObject[] WeekUIs = new GameObject[scheduleLength];

    public static TeamController SingletonInstance = null;
    public static TeamController ThisInstance
    {
        get
        {
            if (SingletonInstance == null)
            {
                GameObject Controller = new GameObject("DefaultController");
                SingletonInstance = Controller.AddComponent<TeamController>();
            }
            return SingletonInstance;
        }
    }

    private void Awake()
    {
        if (SingletonInstance)
        {
            DestroyImmediate(gameObject);
            return;
        }

        SingletonInstance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
       randomNames = new string[]{"Tigers", "Gators", "Wizards", "Pharoahs", "Wildcats", "Reapers", "Wildmen", "Bulldogs"};

        //preset player team values for now
        playerTeam.teamName = "Player";
        playerTeam.wins = 0;
        playerTeam.losses = 0;

        generateTeams();
        generateSchedule();
        printSchedule();
    }

    //Create random teams
    private void generateTeams()
    {
        for(int i = 0; i < scheduleLength; i++)
        {
            Team newTeam = new Team();
            
            //teamName is set to a random value inside randomNames
            newTeam.teamName = randomNames[Mathf.FloorToInt(Random.value * randomNames.Length)];
            newTeam.wins = 0;
            newTeam.losses = 0;
            teams[i] = newTeam;
        }
    }

    private void generateSchedule()
    {
        for (int i = 0; i < scheduleLength; i++)
        {
            Week newWeek = new Week();
            if (Mathf.RoundToInt(Random.value) == 0)
            {
                //Player plays Away
                newWeek.homeTeam = teams[i];
                newWeek.awayTeam = playerTeam;
                newWeek.playerAway = true;
            }
            else
            {
                //Player plays at home
                newWeek.awayTeam = teams[i];
                newWeek.homeTeam = playerTeam;
                newWeek.playerAway = false;
            }
            schedule[i] = newWeek;
        }
    }

    public bool isPlayerAway()
    {
        return schedule[weekCount - 1].playerAway;
    }

    public string getTeamName(int week)
    {
        return this.teams[week].teamName;
    }

    public void updateGame(int homeScore, int awayScore)
    {
        schedule[weekCount - 1].homeScore = homeScore;
        schedule[weekCount - 1].awayScore = awayScore;
        if (homeScore > awayScore)
        {
            schedule[weekCount - 1].homeTeam.wins++;
            schedule[weekCount - 1].awayTeam.losses++;
            if(!schedule[weekCount - 1].playerAway && homeScore == 35)
            {
                playerBigWins++;
            }
        }
        else
        {
            schedule[weekCount - 1].awayTeam.wins++;
            schedule[weekCount - 1].homeTeam.losses++;
            if (schedule[weekCount - 1].playerAway && awayScore == 35)
            {
                playerBigWins++;
            }
        }

        Text[] searchBoxes = WeekUIs[weekCount - 1].GetComponentsInChildren<Text>();
        
        for (int i = 0; i < searchBoxes.Length; i++)
        {
            if (searchBoxes[i].name == "HomeTeam")
            {
                searchBoxes[i].text += " " + homeScore;
            }
            else if (searchBoxes[i].name == "AwayTeam")
            {
                searchBoxes[i].text += " " + awayScore;
            }

        }
        if (weekCount == scheduleLength)
            endSeason();
        else
            weekCount++;
        
    }

    //Check for the end of a season and start over
    private void endSeason()
    {
        //Check for achievements
        //recreate schedule and teams
        clearSchedule();
        generateTeams();
        generateSchedule();
        printSchedule();
        //reset week count
        weekCount = 1;
    }

    private void clearSchedule()
    {
        for(int i = 0; i < scheduleLength; i++)
        {
            Destroy(WeekUIs[i]);
        }
        
    }

    private void printSchedule()
    {
        for(int i = 0; i < scheduleLength; i++)
        {
            GameObject newWeek = Instantiate(WeekUI, scheduleCanvas.transform);
            Text[] textBoxes = newWeek.GetComponentsInChildren<Text>();
            //this is an ugly work around. I need to probably change this
            for(int j = 0; j < textBoxes.Length; j++)
            {
                
                if (textBoxes[j].name == "HomeTeam")
                {
                    textBoxes[j].text = schedule[i].homeTeam.teamName;
                    textBoxes[j].rectTransform.position = scheduleCanvas.transform.position + new Vector3(-320, 350 - (i * 60));
                }
                else if (textBoxes[j].name == "AwayTeam")
                {
                    textBoxes[j].text = schedule[i].awayTeam.teamName;
                    textBoxes[j].rectTransform.position = scheduleCanvas.transform.position + new Vector3(110, 350 - (i * 60));
                }
                else
                {
                    textBoxes[j].rectTransform.position = scheduleCanvas.transform.position + new Vector3(0, 350 - (i * 60));
                }
            }
            newWeek.name = "Week" + (i + 1);
            WeekUIs[i] = newWeek;

        }
    }

    public int getPlayerWins()
    {
        return playerTeam.wins;
    }
    public int getPlayerLosses()
    {
        return playerTeam.losses;
    }
    public int getBigWins()
    {
        return playerBigWins;
    }
}
