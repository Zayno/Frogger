using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuSceneManager : MonoBehaviour
{
    void Update()
    {
        if(Input.anyKeyDown && Time.timeSinceLevelLoad > 0.2)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
            else
            {
                SceneManager.LoadScene("Level1");

            }
        }

    }
}
