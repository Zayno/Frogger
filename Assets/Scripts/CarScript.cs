using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarScript : MonoBehaviour
{
    public enum Direction { Left, Right};
    public float SpawnXPos;
    public float Max_X_Limit;
    public float speed = 1;
    public Direction MyDirection = Direction.Left;
    float LevelDifficultyMultiplier = 1;
    // Start is called before the first frame update
    void Start()
    {
       //LevelDifficultyMultiplier = GameManager.Instance.LevelNumber == GameManager.Level.Level1
       switch(GameManager.Instance?.LevelNumber)
        {
            case GameManager.Level.Level1:
                LevelDifficultyMultiplier = 0.4f;
                break;
            case GameManager.Level.Level2:
                LevelDifficultyMultiplier = 0.6f;
                break;
            case GameManager.Level.Level3:
                LevelDifficultyMultiplier = 0.75f;
                break;

            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(( MyDirection == Direction.Right?  Vector3.right : Vector3.left) * Time.deltaTime * speed * LevelDifficultyMultiplier, Space.World);

        if(MyDirection == Direction.Right)
        {
            if (transform.position.x >= Max_X_Limit)
            {
                transform.position = new Vector3(SpawnXPos, 0.5f, transform.position.z);
            }
        }
        else
        {
            if (transform.position.x <= Max_X_Limit)
            {
                transform.position = new Vector3(SpawnXPos, 0.5f, transform.position.z);
            }
        }

    }
}
