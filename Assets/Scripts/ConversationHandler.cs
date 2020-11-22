using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConversationHandler : MonoBehaviour
{
    public List<string> ConversationStack;

    private int _currentLine = 0;

    public Text ConversationText;

    public Canvas ConversationCanvas;

    private LayerMask _playerMask;

    private bool _isPlayerInRange;


    public List<Book> BooksFire;
    public List<Book> BooksWater;
    public List<Book> BooksAir;
    public List<Book> BooksEarth;
    [HideInInspector]
    public List<Book> CollectableBooks;
    private Book _currentBook;

    private bool _resetConversation;
    private bool _hasIntroPlayed;

    public GameDirector GameDirector;
    public Image UITimer;

    private int _writeEffectLetter = 0;

    private void Start()
    {
        CollectableBooks.Add(BooksFire[UnityEngine.Random.Range(0, BooksFire.Count)]);
        CollectableBooks.Add(BooksWater[UnityEngine.Random.Range(0, BooksWater.Count)]);
        CollectableBooks.Add(BooksAir[UnityEngine.Random.Range(0, BooksAir.Count)]);
        CollectableBooks.Add(BooksEarth[UnityEngine.Random.Range(0, BooksEarth.Count)]);

        _playerMask = LayerMask.NameToLayer("Player");

        ConversationStack.Add("Welcome to my library.");
        ConversationStack.Add("I am Wan Shi Tong.");
        ConversationStack.Add("I'm looking for a few important books. ");
        ConversationStack.Add("I need you to go through the entire library to find those books and return them to me.");
        ConversationStack.Add("Don't waste time by wandering around. ");
        ConversationStack.Add("If you are taking too much time I will completely sink the library to prevent any more books from being stolen.");
        ConversationStack.Add("There is a small oasis at the lowest floor where you can enter the spirit world for a limited time.");
        ConversationStack.Add("Those spirits may know a little more about the whereabouts of the books.");
        ConversationStack.Add("If you take them to me I can give you another hint or 2.");
        PickABook(true);
        ConversationStack.RemoveRange(ConversationStack.Count - 2, 2);
        ConversationStack.TrimExcess();
        ConversationStack.Add("Good luck.");
    }

    private void WaitingConversation()
    {
        ConversationStack.Add("Have you found the book yet?");
        OasisReminder();
    }

    private void OasisReminder()
    {
        ConversationStack.Add("Just a reminder that you can use the oasis at the bottom floor to gather spirits.");
        ConversationStack.Add("If you take them to me I can give you another hint or 2.");
    }

    private void Update()
    {
        if(_isPlayerInRange)
        {
            if (Input.GetKeyDown(KeyCode.E) && _writeEffectLetter < ConversationStack[_currentLine].Length)
                _writeEffectLetter = ConversationStack[_currentLine].Length;
            else if (Input.GetKeyDown(KeyCode.E))
                GoToNextLine();


            ConversationText.text = ConversationStack[_currentLine].Substring(0, _writeEffectLetter);

            if (_writeEffectLetter < ConversationStack[_currentLine].Length)
                _writeEffectLetter++;
        }
    }

    private void GoToNextLine()
    {
        _currentLine++;
        _writeEffectLetter = 0;

        if(_currentLine >= ConversationStack.Count)
        {
            _isPlayerInRange = false;
            ConversationCanvas.gameObject.SetActive(false);
            _currentLine = 0;

            if (!_hasIntroPlayed)
            {
                ConversationStack.Clear();
                PickABook(false);
                _hasIntroPlayed = true;
            }
        }
    }

    private void PickABook(bool randomBook)
    {
        if(CollectableBooks.Count > 0)
        {
            if (randomBook)
            {
                _currentBook = CollectableBooks[UnityEngine.Random.Range(0, CollectableBooks.Count)];
                _currentBook.gameObject.SetActive(true);
            }

            ConversationStack.Add("I need you to find the book about the " + _currentBook.Title + " element.");
            ConversationStack.Add(_currentBook.Description);
            OasisReminder();
        }
        else
        {
            EndGame();
        }

    }

    private void EndGame()
    {
        ConversationStack.Add("You have collected all the books I needed.");
        ConversationStack.Add("Thank you for helping me.");
        ConversationStack.Add("Since you have proven yourself to be trustworthy, you are allowed to freely roam the library.");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == _playerMask)
        {
            if(CollectableBooks.Count <= 0)
                GameDirector.EndGame();

            _isPlayerInRange = true;
            ConversationCanvas.gameObject.SetActive(true);
            _currentLine = 0;

            GameDirector.IsInConversation = true;
            UITimer.color = new Color(0.407f - 0.2f, 0.945f - 0.2f, 1 - 0.2f, 0.784f);


            PlayerStats playerStats = other.GetComponent<PlayerStats>();

            if(_currentBook != null)
            {
                while (playerStats.SpiritsCaptured >= 2 && _currentBook.HintStack.Count > 0)
                {
                    if (_currentBook.HintStack.Count >= 3)
                        ConversationStack.Add("I see you have brought me some spirits. I'll ask them if they have seen the book.");

                    ConversationStack.Add(_currentBook.TransferHint());

                    playerStats.RemoveSpirit(2);
                }
            }
        }
    }

    public void BookDelivered()
    {
        ConversationStack.Clear();
        _currentBook = null;
        ConversationStack.Add("Thank you!");
        ConversationStack.Add("You have done well.");
        PickABook(true);
        _resetConversation = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == _playerMask)
        {
            _isPlayerInRange = false;

            GameDirector.IsInConversation = false;
            UITimer.color = new Color(0.407f, 0.945f, 1, 0.784f);

            ConversationCanvas.gameObject.SetActive(false);
            if(_resetConversation)
            {
                ConversationStack.Clear();
                PickABook(false);
            }
        }
    }

}
