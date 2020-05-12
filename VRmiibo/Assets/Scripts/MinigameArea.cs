﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinigameArea : MonoBehaviour
{
    [SerializeField] private MinigamePanelElements minigamePanelElements;
    [SerializeField] private Sprite minigameSprite;
    [SerializeField] private string minigameText;
    [SerializeField] private int maxPlayersAllowed;
    private int _amountOfPeopleReady = 0;
    private int _amountOfPeopleInside = 0;
    private GameObject colliding;
    private bool visible;
    private int profileSpot;

    //has to be changed to multiplayer purposes
    private void UpdatePanel()
    {
        minigamePanelElements.minigameImage.sprite = minigameSprite;
        minigamePanelElements.waitingText.text = "" + _amountOfPeopleReady + "/" + _amountOfPeopleInside + " are ready to play " + minigameText;
    }
    
    //when a player enters the area
    public void Entered(GameObject other)
    {
        if (_amountOfPeopleInside >= maxPlayersAllowed) return;
        visible = true;
        colliding = other;
        _amountOfPeopleInside++;
        minigamePanelElements.gameObject.SetActive(true);
        UpdatePanel();
        minigamePanelElements.readyButton.onClick.AddListener(ReadyUnReady);
        minigamePanelElements.readyButton.onClick.AddListener(ChangeProfileSprite);
        minigamePanelElements.closeWindowButton.onClick.AddListener(VisibleWindow);
        //minigamePanelElements.closeWindowButton.onClick.AddListener(CloseWindow);
        AddProfileSprite(colliding.GetComponent<Player>().playerSprite);
    }
    
    //when the player closes the window
    private void CloseWindow()
    {
        _amountOfPeopleInside--;
        minigamePanelElements.gameObject.SetActive(false);
        Text readyUnreadyButtonText = minigamePanelElements.readyButton.GetComponentInChildren<Text>();
        if (readyUnreadyButtonText.text == "Unready")
        {
            readyUnreadyButtonText.text = "Ready";
            _amountOfPeopleReady--;
        }
        minigamePanelElements.readyButton.onClick.RemoveListener(ReadyUnReady);
        minigamePanelElements.readyButton.onClick.RemoveListener(ChangeProfileSprite);
        RemoveProfileSprite(colliding.GetComponent<Player>());
        colliding = null;
    }

    //when people press the ready unready button
    public void ReadyUnReady()
    {
        Text readyUnreadyButtonText = minigamePanelElements.readyButton.GetComponentInChildren<Text>();
        if (readyUnreadyButtonText.text == "Ready")
        {
            readyUnreadyButtonText.text = "Unready";
            _amountOfPeopleReady++;
        }
        else
        {
            readyUnreadyButtonText.text = "Ready";
            _amountOfPeopleReady--;
        }
        UpdatePanel();
    }

    private void AddProfileSprite(Sprite sprite)
    {
        for (int i = 0; i < minigamePanelElements.profilePictures.Length; i++)
        {
            if (minigamePanelElements.profilePictures[i].sprite == null)
            {
                profileSpot = i;
                minigamePanelElements.profilePictures[i].sprite = sprite;
                minigamePanelElements.profilePictures[i].color = Color.white;
                break;
            }
        }
    }

    private void ChangeProfileSprite()
    {
        Text readyUnreadyButtonText = minigamePanelElements.readyButton.GetComponentInChildren<Text>();
        if (readyUnreadyButtonText.text == "Ready")
        {
            minigamePanelElements.profilePictures[profileSpot].sprite = colliding.GetComponent<Player>().playerSprite;
        }
        else
        {
            minigamePanelElements.profilePictures[profileSpot].sprite = colliding.GetComponent<Player>().playerReadySprite;
        }
    }
    
    private void RemoveProfileSprite(Player player)
    {
        for (int i = 0; i < minigamePanelElements.profilePictures.Length; i++)
        {
            if (minigamePanelElements.profilePictures[i].sprite == player.playerSprite || minigamePanelElements.profilePictures[i].sprite == player.playerReadySprite)
            {
                minigamePanelElements.profilePictures[i].sprite = null;
                minigamePanelElements.profilePictures[i].color = Color.clear;
                break;
            }
        }
    }

    private void VisibleWindow()
    {
        visible = !visible;
        minigamePanelElements.gameObject.GetComponent<Animator>().SetBool("Visible", visible);
    }
}
