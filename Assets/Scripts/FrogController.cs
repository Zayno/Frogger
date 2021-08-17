using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class FrogController : MonoBehaviour
{

    public GameObject frog;
    public GameObject frogsBody;
    public Ease JumpEaseType;
    Animator MyAnimator;

    public Material blue;
    public Material balckOnRedSpot;
    public Material orangeBlackBlue;
    public Material redGreenBlack;
    public Material yellow;
    public Material yellowOnBlack;

    public GameObject guts;
    bool smashed = false;

    bool IsMoving = false;
    public AudioClip JumpSFX;
    public AudioClip DrownSFX;
    public AudioClip SquishSFX;
    public AudioSource MyAudioSource;
    public bool IsOnFloatie = false;
    public bool IsDead = false;
    public GameObject Fireworks;
    public GameObject ParticlesGO;
    public GameObject CarHitParticleGO;
    readonly float EastLimit = 9;
    readonly float WestLimit = -9;
    readonly float SouthLimit = -14;
    private void Awake()
    {
        MyAnimator = frog.GetComponent<Animator>();
    }

    private void Start()
    {
        transform.position = new Vector3(Random.Range((int)-9, 9), 0, -14.0f);//randomize frog position at startup
    }



    public void Hop()
    {
        MyAnimator.Play("Jump", -1, 0f);
    }

    public void KillFrog()
    {
        IsDead = true;
        GetComponent<BoxCollider>().enabled = false;
        GameManager.Instance.OnFrogDied();
    }

    public void DiedFromWater()
    {
        if (transform.parent != null)
            transform.parent = null;

        KillFrog();
        PlaySoundOnce(DrownSFX);
        MyAnimator.Play("Smashed", -1, 0f);
        transform.DOMoveY(-1, 3);
        ParticlesGO.SetActive(true);
    }

    public void Smashed2()
    {
        KillFrog();
        MyAnimator.Play("Smashed", -1, 0f);
        SpreadGuts();
        PlaySoundOnce(SquishSFX);
        CarHitParticleGO.SetActive(true);
    }

    void SpreadGuts()
    {
        smashed = false;
        if (!smashed)
        {
            Instantiate(guts, frog.transform.position, frog.transform.rotation);
            smashed = true;
        }
    }

    public void ActivateFireworks()
    {
        if(Fireworks != null)
        {
            Fireworks.SetActive(true);

        }
    }

    //this gets called when the frog movement has ended and we can now see where it landed
    void OnMoveComplete()
    {
        IsMoving = false;
        RaycastHit hit;

        if (IsDead == false && Physics.Raycast(transform.position, -Vector3.up, out hit, 1))
        {
            if (hit.collider.tag == "FloatingObj")
            {
                IsOnFloatie = true;
                transform.parent = hit.transform.parent;
            }
            else if (hit.collider.tag == "FloatingTurtle")
            {
                IsOnFloatie = true;
                transform.parent = hit.transform.parent.parent.parent.parent;
            }
            else if (hit.collider.tag == "Water")
            {
                DiedFromWater();
                transform.parent = null;
            }
            else if (hit.collider.tag == "FinishLine")
            {
                GameManager.Instance.OnReachedFinishLine();
            }
            else
            {
                IsOnFloatie = false;
                transform.parent = null;
            }
        }
    }

    public void MoveForward()
    {
        if (IsMoving)
            return;

        IsMoving = true;
        PlaySoundOnce(JumpSFX);

        transform.rotation = Quaternion.Euler(0, 0, 0);
        Hop();
        Vector3 NewPos = transform.position + Vector3.forward;
        transform.DOMove(NewPos, 0.2f).SetEase(JumpEaseType).OnComplete(OnMoveComplete);
        transform.parent = null;

    }

    public void MoveBack()
    {
        if (transform.position.z >= SouthLimit + 0.95f)
        {
            if (IsMoving)
                return;

            IsMoving = true;
            PlaySoundOnce(JumpSFX);

            transform.rotation = Quaternion.Euler(0, 180, 0);
            Hop();
            Vector3 NewPos = transform.position - Vector3.forward;
            transform.DOMove(NewPos, 0.2f).SetEase(JumpEaseType).OnComplete(OnMoveComplete);
            transform.parent = null;
        }
      

    }

    public void MoveLeft()
    {
        if(transform.position.x >= WestLimit + 0.95f)
        {
            if (IsMoving)
                return;

            IsMoving = true;
            PlaySoundOnce(JumpSFX);


            transform.rotation = Quaternion.Euler(0, -90, 0);
            Hop();
            Vector3 NewPos = transform.position - Vector3.right;
            transform.DOMove(NewPos, 0.2f).SetEase(JumpEaseType).OnComplete(OnMoveComplete);
        }

    }

    public void MoveRight()
    {
        if(transform.position.x <= EastLimit - 0.95f)
        {
            if (IsMoving)
                return;

            IsMoving = true;
            PlaySoundOnce(JumpSFX);


            transform.rotation = Quaternion.Euler(0, 90, 0);
            Hop();
            Vector3 NewPos = transform.position + Vector3.right;
            transform.DOMove(NewPos, 0.2f).SetEase(JumpEaseType).OnComplete(OnMoveComplete);
        }

    }

    public void MoveRightOnFlotingObj()
    {
        if (IsMoving)
            return;

        IsMoving = true;
        PlaySoundOnce(JumpSFX);


        transform.rotation = Quaternion.Euler(0, 90, 0);
        Hop();
        Vector3 NewPos = transform.localPosition + Vector3.right;
        transform.DOLocalMove(NewPos, 0.2f).SetEase(JumpEaseType).OnComplete(OnMoveComplete);
    }

    public void MoveLeftOnFlotingObj()
    {
        if (IsMoving)
            return;

        IsMoving = true;
        PlaySoundOnce(JumpSFX);

        transform.rotation = Quaternion.Euler(0, -90, 0);
        Hop();
        Vector3 NewPos = transform.localPosition - Vector3.right;
        transform.DOLocalMove(NewPos, 0.2f).SetEase(JumpEaseType).OnComplete(OnMoveComplete);
    }

    void PlaySoundOnce(AudioClip AuidoToPlay)
    {
        MyAudioSource.clip = AuidoToPlay;
        MyAudioSource.Play();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "KillOnCollide")
        {
            Smashed2();
        }
        else if (other.tag == "Wall")
        {
            Debug.Log("WALL");
            DiedFromWater();
        }
    }

}