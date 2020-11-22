using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public int SpiritsCaptured;

    //public List<int> BookIDs;

    public GrabBookCollider GrabBookCollider;

    public List<Image> UISpirits;

    public List<Image> UISpiritFullGlow;

    public TriggerSpiritWorld TriggerSpiritWorld;

    private void Start()
    {
        UpdateUI();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.X))
            GrabBook();
    }

    private void GrabBook()
    {
        if(GrabBookCollider.BookInRange != null)
        {
            //BookIDs.Add(GrabBookCollider.GetBookID());
            GrabBookCollider.BookGrabbed();
        }
    }

    public void AddSpirit(int amount)
    {
        SpiritsCaptured = Mathf.Min(SpiritsCaptured + amount, 5);
        UpdateUI();
    }

    public void RemoveSpirit(int amount)
    {
        SpiritsCaptured = Mathf.Max(SpiritsCaptured - amount, 0);
        UpdateUI();
    }

    private void UpdateUI()
    {
        foreach (Image image in UISpirits)
            image.gameObject.SetActive(false);

        for (int i = 0; i < SpiritsCaptured; i++)
            UISpirits[i].gameObject.SetActive(true);

        if (SpiritsCaptured >= 5)
        {
            ChangeUISpiritGlow(true);
            TriggerSpiritWorld.EnterNormalWorld();
        }
        else
            ChangeUISpiritGlow(false);
    }

    private void ChangeUISpiritGlow(bool state)
    {
        foreach (var UIelement in UISpiritFullGlow)
            UIelement.gameObject.SetActive(state);
    }
}
