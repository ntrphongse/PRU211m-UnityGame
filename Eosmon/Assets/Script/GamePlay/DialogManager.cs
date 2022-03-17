using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public enum DialogState { Start, PlayerAction, PLayerMove, EnemyMove, Busy, Dialog, FreeRoam }


public class DialogManager : MonoBehaviour
{
    [SerializeField] GameObject dialogBox;
    [SerializeField] Text dialogText;
    [SerializeField] int letterPerSecond;

    DialogState state;

    public static DialogManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }
    public event Action OnShowDialog;
    public event Action OnCloseDialog;

    Dialog dialog;
    int currentLine = 0;
    bool isTyping=true;

    private void Start()
    {
        OnShowDialog += () => { state = DialogState.Dialog; };
        OnCloseDialog += () => { if(state==DialogState.Dialog) state = DialogState.FreeRoam; };
    }

    public IEnumerator ShowDialog(Dialog dialog) 
    {
        yield return new WaitForEndOfFrame();
        OnShowDialog?.Invoke();
        this.dialog = dialog;
        dialogBox.SetActive(true);
        StartCoroutine(TypeDialog(dialog.Lines[0]));
    }

    private void Update()
    {
        if (state == DialogState.Dialog)
        {
            HandleUpdate();
        }
        
    }

    public void HandleUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Z) && !isTyping)
        {
            ++currentLine;
            if (currentLine < dialog.Lines.Count) { StartCoroutine(TypeDialog(dialog.Lines[currentLine])); }
            else
            {
                currentLine = 0;
                dialogBox.SetActive(false);
                OnCloseDialog?.Invoke();
            }
        }
    }

    public IEnumerator TypeDialog(string line)
    {
        isTyping = true;
        dialogText.text = "";
        foreach (var item in line.ToCharArray())
        {
            dialogText.text += item;
            yield return new WaitForSeconds(1f / letterPerSecond);
        }
        isTyping = false;
    }
}
