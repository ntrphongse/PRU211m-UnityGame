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

    [SerializeField] Image background;


    private string _name;
    public void Update()
    {
    }
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
        if (isPlayerUnit)
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
            background.transform.DOShakePosition(15f, 7f);
            image.transform.DOLocalMoveX(250f, 13f).OnComplete(stopBossWalking);
        }
        else
        {
            image.transform.DOLocalMoveX(originalPos.x, 1f);
        }
    }

    public void PlayAttacAnimation()
    {
        var seq = DOTween.Sequence();
        if (isPlayerUnit)
        {
            seq.Append(image.transform.DOLocalMoveX(originalPos.x + 50f, 0.25f));
        }
        else
        {
            if (_name == "01101100011010000110101101110000")
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
        else
        {
            seq.Append(image.transform.DOLocalMoveX(originalPos.x, 0.25f));
        }
    }

    public void PlayHitAnimation()
    {
        var seq = DOTween.Sequence();
        hitSound.Play();
        seq.Append(image.DOColor(Color.gray, 0.1f));
        seq.Append(image.DOColor(ogColor, 0.1f));
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
}
