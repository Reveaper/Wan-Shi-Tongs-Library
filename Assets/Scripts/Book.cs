using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book : MonoBehaviour
{
    public string Title;
    public string Description;
    //public int ID;
    [SerializeField]
    private GameObject Notifier;

    public List<string> HintStack;

    public List<GameObject> UIHints;

    public void EnableNotifier()
    {
        Notifier.SetActive(true);
    }

    public void DisableNotifier()
    {
        Notifier.SetActive(false);
    }

    public string TransferHint()
    {
        if (HintStack.Count > 0)
        {
            UIHints[0].SetActive(true);
            UIHints.RemoveAt(0);
            string giveString = HintStack[0];
            HintStack.RemoveAt(0);
            return giveString;
        }
        else
            return "I have no idea what the spirit just said, likely due to an error.";
 
    }
}
