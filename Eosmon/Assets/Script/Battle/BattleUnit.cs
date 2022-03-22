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

    public void Setup()
    {
        Lecturer = new Lecturer(_base, level);
        if (isPlayerUnit)
        {
            image.sprite = Lecturer.Base.FrontSprite;
        }
        else
        {
            image.sprite = Lecturer.Base.BackSprite;
        }
        image.color = ogColor;
        PlayEnterAnimation();
    }

    public void PlayEnterAnimation()
    {
        if (isPlayerUnit)
        {
            image.transform.localPosition = new Vector3(-500f, originalPos.y);
        }
        else
        {
            image.transform.localPosition = new Vector3(500f, originalPos.y);
        }

        image.transform.DOLocalMoveX(originalPos.x, 1f);
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
            seq.Append(image.transform.DOLocalMoveX(originalPos.x - 50f, 0.25f));
        }
        seq.Append(image.transform.DOLocalMoveX(originalPos.x, 0.25f));
    }

    public void PlayHitAnimation()
    {
        var seq = DOTween.Sequence();
        hitSound.Play();
        seq.Append(image.DOColor(Color.gray, 0.1f));
        seq.Append(image.DOColor(ogColor, 0.1f));
    }


    public void PLayFaintAnimation()
    {
        var seq = DOTween.Sequence();
        seq.Append(image.transform.DOLocalMoveY(originalPos.y - 150f, 0.5f));
        seq.Join(image.DOFade(0f, 0.5f));
    }
}
