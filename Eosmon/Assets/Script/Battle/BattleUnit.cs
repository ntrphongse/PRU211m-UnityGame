using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class BattleUnit : MonoBehaviour
{
    [SerializeField] MobBase _base;
    [SerializeField] int level;
    [SerializeField] bool isPlayerUnit;

    [SerializeField] AudioSource hitSound;

    [SerializeField] Animator animator;
    [SerializeField] AudioSource bossWalkingSound;
    [SerializeField] AudioSource finalMovementSound;


    [SerializeField] Image background;

    private string _name;
    Image image;
    Vector3 originalPos;
    Color ogColor;
    private void Awake()
    {
        image = GetComponent<Image>();
        originalPos = image.transform.localPosition;
        ogColor = image.color;
    }

    public Lecturer Lecturer { get; set; }

    public void Setup(string npcName)
    {
        Lecturer = new Lecturer(_base, level);
        Lecturer.Base.SetName(npcName);
        _name = npcName;
        if (npcName == "01101100011010000110101101110000")
        {
            Lecturer.Level = 999;
            Lecturer.Base.SetMaxKp(9999);
        }
        if (npcName == "Seph")
        {
            Lecturer.Level = 999;
            Lecturer.Base.SetMaxKp(9999);
        }
        if (isPlayerUnit)
        {
            image.sprite = Lecturer.Base.FrontSprite;
        }
        else
        {
            image.sprite = Lecturer.Base.BackSprite;
        }
        image.color = ogColor;
        PlayEnterAnimation(npcName);
    }
    private void stopBossWalking()
    {
        animator.SetBool("IsBossWalking", false);
        bossWalkingSound.Stop();
    }

    public void PlayEnterAnimation(string npcName)
    {
        if (isPlayerUnit && npcName != "Hero")
        {
            image.transform.localPosition = new Vector3(-500f, originalPos.y);
        }
        else
        {
            image.transform.localPosition = new Vector3(500f, originalPos.y);
        }
        if (npcName == "01101100011010000110101101110000")
        {
            bossWalkingSound.Play();
            background.transform.DOShakePosition(18f, 10f);
            image.transform.DOLocalMoveX(250f, 15f).OnComplete(stopBossWalking);
        }
        else if (npcName == "Hero")
        {
            image.transform.DOLocalMoveX(-230f, 1f);
        }
        else if (npcName == "Seph")
        {
            image.transform.DOLocalMoveX(250f, 1f);
        }
        else
        {
            image.transform.DOLocalMoveX(originalPos.x, 1f);
        }
    }

    public void PlayAttacAnimation()
    {
        var seq = DOTween.Sequence();
        if (isPlayerUnit && _name != "Hero")
        {
            seq.Append(image.transform.DOLocalMoveX(originalPos.x + 50f, 0.25f));
        }
        else if (isPlayerUnit && _name == "Hero")
        {
            seq.Append(image.transform.DOLocalMoveX(-230f + 50f, 0.25f));
        }
        else
        {
            if (_name == "01101100011010000110101101110000")
            {
                seq.Append(image.transform.DOLocalMoveX(250f - 50f, 0.25f));
            }
            else if (_name == "Seph")
            {
                seq.Append(image.transform.DOLocalMoveX(250f - 50f, 0.25f));
            }
            else
            {
                seq.Append(image.transform.DOLocalMoveX(originalPos.x - 50f, 0.25f));
            }
        }
        if (_name == "01101100011010000110101101110000")
        {
            seq.Append(image.transform.DOLocalMoveX(250f, 0.25f));
        }
        else if (_name == "Seph")
        {
            seq.Append(image.transform.DOLocalMoveX(250f, 0.25f));
        }
        else if (_name == "Hero")
        {
            seq.Append(image.transform.DOLocalMoveX(-230f, 0.25f));
        }
        else
        {
            seq.Append(image.transform.DOLocalMoveX(originalPos.x, 0.25f));
        }
    }

    public void PlayFinalMovementAnimation()
    {

        image.transform.DOLocalMoveX(0f, 0.25f).OnComplete(() =>
        {
            finalMovementSound.Play();
            animator.Play("FinalMovement");
        });
    }

    public void PlayAttacSeph()
    {
        var seq = DOTween.Sequence();

        seq.Append(image.transform.DOLocalMoveX(0f, 0.25f).OnComplete(() =>
        {
            image.rectTransform.sizeDelta = new Vector2(390, 174);
            animator.Play("SephAttac");
        }));
    }

    public void PlayAttacCloud()
    {
        var seq = DOTween.Sequence();
        seq.Append(image.transform.DOLocalMoveX(0f, 0.25f).OnComplete(() =>
        {
            image.rectTransform.sizeDelta = new Vector2(400, 250);
            animator.Play("CloudAttac");
        }));
    }


    public void SetCloudNormal()
    {
        var seq = DOTween.Sequence();
        seq.Append(image.transform.DOLocalMoveX(-230f, 0.25f).OnComplete(() =>
        {
            image.rectTransform.sizeDelta = new Vector2(130, 174);
        }));
    }
    public void PlaySephLimAnimation()
    {
        image.transform.DOLocalMoveX(-100f, 0.25f).OnComplete(() =>
        {
            animator.SetBool("IsSephLimit", true);
            finalMovementSound.Play();
            //animator.Play("SephLimit");
            image.rectTransform.sizeDelta = new Vector2(400, 400);
        });
    }

    public void SetSephNormal()
    {
        var seq = DOTween.Sequence();
        seq.Append(image.transform.DOLocalMoveX(250f, 0.25f).OnComplete(() =>
        {
            image.rectTransform.sizeDelta = new Vector2(165, 175);
        }));
    }

    public void StopSephLimAnimation()
    {
        image.rectTransform.sizeDelta = new Vector2(165, 175);
        animator.SetBool("IsSephLimit", false);
    }

    public void MoveEvent()
    {
        image.transform.DOLocalMoveX(200f, 0.25f);
    }

    public void FinishEvent()
    {
        image.transform.DOLocalMoveX(-230f, 1f);
    }

    public void PlayHitAnimation()
    {
        var seq = DOTween.Sequence();
        hitSound.Play();
        seq.Append(image.DOColor(Color.gray, 0.1f));
        seq.Append(image.DOColor(ogColor, 0.1f));
    }

    public void LoopHitAnimation()
    {
        var seq = DOTween.Sequence();
        for (int i = 0; i < 30; i++)
        {
            seq.Append(image.DOColor(Color.gray, 0.1f));
            seq.Append(image.DOColor(ogColor, 0.1f));
        }
    }
    public void PlayHealAnimation()
    {
        var seq = DOTween.Sequence();
        seq.Append(image.DOColor(Color.green, 0.1f));
        seq.Append(image.DOColor(ogColor, 0.1f));
        seq.Append(image.DOColor(Color.green, 0.1f));
        seq.Append(image.DOColor(ogColor, 0.1f));
        seq.Append(image.DOColor(Color.green, 0.1f));
        seq.Append(image.DOColor(ogColor, 0.1f));
        seq.Append(image.DOColor(Color.green, 0.1f));
        seq.Append(image.DOColor(ogColor, 0.1f));
    }

    public void PLayFaintAnimation()
    {
        var seq = DOTween.Sequence();
        seq.Append(image.transform.DOLocalMoveY(originalPos.y - 150f, 0.5f));
        seq.Join(image.DOFade(0f, 0.5f));
    }

    public void PlayPhaseTwoBeginAnimation()
    {

    }
}
