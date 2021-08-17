using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum Level { none, Level1 = 1, Level2 = 2, Level3 = 3};
    public Level LevelNumber = Level.none;//Assigned in inspector
    public GameObject MyCamera;
    public FrogController MyFrog;
    public bool HasReachedFinishLine = false;
    bool PauseInput = false;

    void Start()
    {
        Input.ResetInputAxes();
        SaveData.CurrentLevelNumber = (int)LevelNumber;
        UISceneManager.Instance?.UpdateTexts();

    }

    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        SceneManager.LoadScene("UIScene", LoadSceneMode.Additive);
    }

    public void OnReachedFinishLine()
    {
        PauseInput = true;
        HasReachedFinishLine = true;
        StartCoroutine(WinCoroutine());
    }

    public void OnFrogDied()
    {
        PauseInput = true;

        StartCoroutine(RestartCoroutine());

    }

    IEnumerator WinCoroutine()
    {
        MyCamera.GetComponent<CameraScript>().DoWinAnim();
        MyFrog.ActivateFireworks();
        yield return new WaitForSeconds(2);

        if(LevelNumber == Level.Level1)
        {
            SceneManager.LoadScene("Level2");
        }
        else if(LevelNumber == Level.Level2)
        {
            SceneManager.LoadScene("Level3");
        }
        else if(LevelNumber == Level.Level3)
        {
            SaveData.TotalGameWins++;
            UISceneManager.Instance?.UpdateTexts();

            SceneManager.LoadScene("Level1");
        }
    }
    IEnumerator RestartCoroutine()
    {
        SaveData.TotalDeaths++;
        UISceneManager.Instance?.UpdateTexts();

        MyCamera.GetComponent<CameraScript>().DoWinAnim();
        yield return new WaitForSeconds(2);

        SceneManager.LoadScene("Level1");

    }

    // Update is called once per frame
    void Update()
    {
        if (PauseInput)
            return;

        if (Input.anyKeyDown && Time.timeSinceLevelLoad > 0.2f)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
            else
            {
                if (MyFrog.IsDead || HasReachedFinishLine)
                {
                    Input.ResetInputAxes();

                    SceneManager.LoadScene("MenuScene");
                }
                else
                {

                    if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
                    {
                        MyFrog.MoveForward();
                    }
                    else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
                    {
                        MyFrog.MoveBack();
                    }
                    else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
                    {
                        if (MyFrog.IsOnFloatie)
                        {
                            MyFrog.MoveLeftOnFlotingObj();
                        }
                        else
                        {
                            MyFrog.MoveLeft();
                        }
                    }
                    else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
                    {
                        if (MyFrog.IsOnFloatie)
                        {
                            MyFrog.MoveRightOnFlotingObj();
                        }
                        else
                        {
                            MyFrog.MoveRight();
                        }
                    }
                }
            }
        }
    }
}
