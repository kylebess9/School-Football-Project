using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Base Achievement class
//Holds basic name, complete boolean, and description
public class Achievement
{
    public string achievementName;
    private bool isComplete;
    public string achievementDescription;

    public void isDone(bool boolIn)
    {
        this.isComplete = boolIn;
    }

    public bool achieved()
    {
        return isComplete;
    }

    public Achievement(string nameIn = "Default", string desc = "None")
    {
        isComplete = false;
        achievementName = nameIn;
        achievementDescription = desc;
    }
}
//I split off the numeric achievements (these will be the primary goals in the is game)
//I wanted to be able to make different achievements have different goals

//THIS NEEDS TO BE CLEANED UP
//CANT ACCESS NumericAchievenments in an Achievement Array
public class NumericAchievement : Achievement
{
    private int currentNumber;
    private int goal;

    public NumericAchievement(int goalIn, string name, string descIn) : base(name, descIn)
    {
        goal = goalIn;
    }

    public void updateNumber(int number)
    {
        currentNumber = number;
        if(currentNumber >= goal)
        {
            this.isDone(true);
        }
    }

    
}

public class AchievementHandler : MonoBehaviour {

    //Stats to follow
    public int playerWins = 0;
    public int playerLosses = 0;
    public int playerScore = 0;
    public int playerScoredOn = 0;

    //Achievement objects

    private Achievement firstWin = new Achievement("First Win", "Win a game");
    private Achievement firstLoss = new Achievement("First Lose", "Try again");
    //private NumericAchievement hundredPoints = new NumericAchievement(100, "100 Points", "Hit 100 points!");
    private Achievement bigWin = new Achievement("Big Win", "Win a game by 35");
    private static int numOfAchievements = 4;

    //Hold achievement objects
    public Achievement[] achievementList = new Achievement[numOfAchievements];


    //Canvas to print to
    public Canvas achievementCanvas = null;
    //UIElement to print
    public GameObject UIAchieve = null;
    private GameObject[] UIAchieves = new GameObject[numOfAchievements];

    //Create a singleton
    public static AchievementHandler SingletonInstance = null;
    public static AchievementHandler ThisInstance
    {
        get
        {
            if (SingletonInstance == null)
            {
                GameObject Controller = new GameObject("DefaultController");
                SingletonInstance = Controller.AddComponent<AchievementHandler>();
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
        achievementList[0] = firstWin;
        achievementList[1] = firstLoss;
        //achievementList[2] = hundredPoints;
        achievementList[2] = bigWin;

        printAchievements();
    }
    //Definitely needs to be reviewed, activating by id is not ideal
    public void updateAchievement(int id, bool boolIN)
    {
        achievementList[id].isDone(boolIN);
        UIAchieves[id].GetComponentInChildren<UIAchievementCheck>().complete();
    }

    public void updateAchievement(int id, int numUpdate)
    {
        //Cant access numericAchievements
        //Probably need to go back over my process before proceeding with these types
    }

    public void updateStatistics()
    {
        playerWins = TeamController.ThisInstance.getPlayerWins();
        playerLosses = TeamController.ThisInstance.getPlayerLosses();

        if (!achievementList[0].achieved() && playerWins > 0)
        {
            updateAchievement(0, true);
        }
        if (!achievementList[1].achieved() && playerLosses > 0)
        {
            updateAchievement(1, true);
        }
        if (!achievementList[2].achieved() && TeamController.ThisInstance.getBigWins() > 0)
        {
            updateAchievement(2, true);
        }
    }

    private void printAchievements()
    {
        for(int i = 0; i < achievementList.Length; i++)
        {
            if(achievementList[i] == null)
            {
                break;
            }
            //Create UIElement
            GameObject newAchieve = Instantiate(UIAchieve, achievementCanvas.transform);
            //Set achieve position and text
            Text[] textBoxes = newAchieve.GetComponentsInChildren<Text>();
            for (int j = 0; j < textBoxes.Length; j++)
            {

                if (textBoxes[j].name == "AchievementName")
                {
                    textBoxes[j].text = achievementList[i].achievementName;
                    textBoxes[j].rectTransform.position = achievementCanvas.transform.position + new Vector3(-250, 320 - (i * 100));
                }
                else if (textBoxes[j].name == "DescriptionText")
                {
                    textBoxes[j].text = achievementList[i].achievementDescription;
                    textBoxes[j].rectTransform.position = achievementCanvas.transform.position + new Vector3(-250, 290 - (i * 100));
                }
            }
            //move the image
            newAchieve.GetComponentInChildren<Image>().transform.position = achievementCanvas.transform.position + new Vector3(200, 315 - (i * 100));
            UIAchieves[i] = newAchieve; 
        }
    }


}
