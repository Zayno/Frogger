using System.Collections;
using DG.Tweening;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public GameObject Frog;
    public Transform FrogCamPos;
    public Transform FrogEye;
    Vector3 InitialVector;
    bool WinCam = false;
    // Start is called before the first frame update
    void Start()
    {
        InitialVector = transform.position - Frog.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (WinCam)
        {
            transform.LookAt(FrogEye, Vector3.up);
        }
        else
        {
            transform.position = Frog.transform.position + InitialVector;
        }

    }

    public void DoWinAnim()
    {
        GetComponent<Camera>().orthographic = false;
        WinCam = true;
        transform.DOMove(FrogCamPos.position, 1.0f).SetEase(Ease.InOutCirc);
    }
}
