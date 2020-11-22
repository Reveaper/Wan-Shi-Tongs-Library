using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabBookCollider : MonoBehaviour
{
    public Book BookInRange;

    private LayerMask _bookMask;

    [SerializeField]
    private ConversationHandler _conversationHandler;

    public UI_Paper UIPaper;

    private void Start()
    {
        _bookMask = LayerMask.NameToLayer("Book");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == _bookMask)
        {
            BookInRange = other.GetComponent<Book>();
            BookInRange.EnableNotifier();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == _bookMask)
        {
            BookInRange.DisableNotifier();
            BookInRange = null;
        }
    }

    public void BookGrabbed()
    {
        _conversationHandler.CollectableBooks.Remove(BookInRange);
        _conversationHandler.BookDelivered();
        Destroy(BookInRange.gameObject);
        BookInRange = null;
        UIPaper.ClearHints();
    }
    /*
    public int GetBookID()
    {
        return BookInRange.ID;
    }*/
}
