using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BattleDialogBox : MonoBehaviour
{
    [SerializeField] Color hightlightedColour;


    [SerializeField] Text dialogText;
    [SerializeField] int letterPerSecond;
    [SerializeField] GameObject moveSelector;
    [SerializeField] GameObject actionSelector;

    [SerializeField] List<Text> actionsText;
    [SerializeField] List<Text> moveText;

    public void SetColor(Color color)
    {
        dialogText.color = color;
    }
    public void SetDialog(string dialog)
    {
        dialogText.text = dialog;
    }

    public IEnumerator TypeDialog(string dialog)
    {
        dialogText.text = "";
        foreach (var item in dialog)
        {
            dialogText.text += item;
            yield return new WaitForSeconds(1f / letterPerSecond);
        }
    }
    public void EnableDialogText(bool enabled)
    {
        dialogText.enabled = enabled;
    }

    public void EnabledActionSelector(bool enabled)
    {
        actionSelector.SetActive(enabled);
    }
    public void EnabledMoveSelector(bool enabled)
    {
        moveSelector.SetActive(enabled);
    }

    public void UpdateActionSelection(int selectedAction)
    {
        for (int i = 0; i < actionsText.Count; i++)
        {
            if (i == selectedAction)
            {
                actionsText[i].color = hightlightedColour;
            }
            else
            {
                actionsText[i].color = Color.black;

            }
        }
    }
    public void UpdateMoveSelection(int selectedMove)
    {
        for (int i = 0; i < moveText.Count; i++)
        {
            if (i == selectedMove)
            {
                moveText[i].color = hightlightedColour;
            }
            else
            {
                moveText[i].color = Color.black;

            }
        }
    }

    public void setActionText(string[] actions)
    {
        for (int i = 0; i < actions.Length; i++)
        {
            actionsText[i].text = actions[i];
        }
    }
    public void setMoveNames(SuggestedAnswers answers)
    {
        for (int i = 0; i < 4; i++)
        {
            switch (i)
            {
                case 0: moveText[i].text = "A. " + answers.A; break;
                case 1: moveText[i].text = "B. " + answers.B; break;
                case 2: moveText[i].text = "C. " + answers.C; break;
                case 3: moveText[i].text = "D. " + answers.D; break;
            }
        }
    }
}
