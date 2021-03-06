﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject cardTemplate;
    public List<Card> cardList;
    public Card card;
    public Text title;
    public TextMeshProUGUI description;
    public RawImage image;
    public RawImage imageDup;
    public Card cardBack;
    public string titleBack, descriptionBack;
    public Texture2D imageBack;
    public GameObject check, cross, timeoutSprite;
    public GameObject MainText;
    public GameObject timeFrame;
    public GameObject endPanel;
    public Text scoreText;
    public bool waiting;
    public static int score;
    public int nextSceneIndex;
    public TextMeshProUGUI questionIdxShow;
    public GameObject frameIdx;
    public TextMeshProUGUI timeText;
    private float time;
    private bool timeOn , isTimeOut;
    public AudioSource trueSound, wrongSound,timeUp;

    public static GameManager instance = null;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void PlayTheGame()
    {
        timeOn = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        nextSceneIndex = 1;
        questionIdxShow.text = nextSceneIndex + "/20";
        questionIdxShow.gameObject.SetActive(true);
        isTimeOut = false;
        timeOn = false;
        time = 15.2f;
        cardList = new List<Card>();
        waiting = false;
        AddCard();
        
        //NextCard();
        //ShowCard();
        Invoke("NextCard", 0.2f);
        Invoke("ShowCard", 0.3f);
        Invoke("DuplicateCard", 0.3f);
        score = 0;
        Time.timeScale = 1;
    }

    private void Update()
    {
        timeText.text = ((int)time).ToString();
        if (timeOn)
        {
            time -= Time.deltaTime;
            if (time <= 0)
            {
                isTimeOut = true;
                AnswerFalse();
            }
        }
    }

    public void AnswerTrue()
    {
        if (!waiting)
        {
            timeOn = false;
            time = 15.2f;
            trueSound.Play();
            waiting = true;
            check.SetActive(true);
            MainText.SetActive(false);
            timeFrame.SetActive(false);
            questionIdxShow.gameObject.SetActive(false);
            frameIdx.SetActive(false);
            description.gameObject.SetActive(false);
            timeText.gameObject.SetActive(false);
            Invoke("RemovedCheck", 2.1f);
            Debug.Log("true");
            score += 1;
            nextSceneIndex++;
            questionIdxShow.text = nextSceneIndex + "/20";
            Invoke("ShowCard", 2);
        }
    }

    public void DuplicateCard()
    {
        imageDup.texture = image.texture;
    }
    public void AnswerFalse()
    {
        if (!waiting)
        {
            timeOn = false;
            time = 15.2f;
            
            waiting = true;
            if (isTimeOut) {
                timeUp.Play();
                timeoutSprite.SetActive(true);
            }
            else{
                wrongSound.Play();
                cross.SetActive(true);
            }
            
            isTimeOut = false;
            MainText.SetActive(false);
            timeFrame.SetActive(false);
            questionIdxShow.gameObject.SetActive(false);
            frameIdx.SetActive(false);
            description.gameObject.SetActive(false);
            timeText.gameObject.SetActive(false);
            Invoke("RemovedCross", 2.1f);
            Debug.Log("false");
            Invoke("ShowCard", 2);
            nextSceneIndex++;
            questionIdxShow.text = nextSceneIndex + "/20";
        }
    }

    public void ShowCard()
    {
        if (cardBack != null)
        {
            card.tagId = cardBack.tagId;
            //title.text = titleBack.text;
            description.text = descriptionBack.ToUpper();
            //Sprite curSprite = Sprite.Create(imageBack, new Rect(0, 0, imageBack.width, imageBack.height), new Vector2(0.5f, 0.5f));
            //image.sprite = Sprite.Create(imageBack, new Rect(0, 0, imageBack.width, imageBack.height), new Vector2(0.5f, 0.5f));
            image.texture = imageBack;
            NextCard();
        }
        else
        {
            Destroy(card.gameObject);
            EndGame();
        }

        if(nextSceneIndex >= 21)
        {
            Destroy(card.gameObject);
            EndGame();
        }
    }

    public void NextCard()
    {
        if (cardList.Count != 0)
        {
            int index = Random.Range(0, cardList.ToArray().Length);
            Card nextCardObject = cardList[index];
            Card nextCard = nextCardObject;
            cardBack.tagId = nextCard.tagId;
            //titleBack.text = nextCard.title.text;
            descriptionBack = nextCard.descriptionString;
            imageBack = nextCard.image;
            //imageBack.overrideSprite = Sprite.Create(nextCard.image, new Rect(0, 0, nextCard.image.width, nextCard.image.height), new Vector2(0.5f, 0.5f));
            //RemoveCard(nextCard.id);
            cardList.RemoveAt(index);
        }
        else
        {
            Destroy(cardBack.gameObject);
        }
    }

    public void AddProfitCard(string id,string description,Texture2D image)
    {
        Card currCard = new Card();
        //currCard.title.text = title;
        currCard.id = id;
        currCard.descriptionString = description;
        currCard.image = image;
        //currCard.image.overrideSprite = Sprite.Create(image, new Rect(0,0,image.width,image.height),new Vector2(0.5f, 0.5f));
        currCard.tagId = "Profit";
        cardList.Add(currCard);
        //Instantiate(cardTemplate, card.transform.position, Quaternion.identity);
    }

    public void AddGrowthCard(string id,string description, Texture2D image)
    {
        Card currCard = new Card();
        //currCard.title.text = title;
        currCard.id = id;
        currCard.descriptionString = description;
        currCard.image = image;
        //currCard.image.overrideSprite = Sprite.Create(image, new Rect(0, 0, image.width, image.height), new Vector2(0.5f, 0.5f));
        currCard.tagId = "Growth";
        cardList.Add(currCard);
        //Instantiate(cardTemplate, card.transform.position, Quaternion.identity);
    }

    private void RemovedCheck()
    {
        timeOn = true;
        waiting = false;
        check.gameObject.SetActive(false);
        timeFrame.SetActive(true);
        questionIdxShow.gameObject.SetActive(true);
        MainText.SetActive(true);
        frameIdx.SetActive(true);
        description.gameObject.SetActive(true);
        timeText.gameObject.SetActive(true);
    }

    private void RemovedCross()
    {
        timeOn = true;
        waiting = false;
        cross.gameObject.SetActive(false);
        timeFrame.SetActive(true);
        questionIdxShow.gameObject.SetActive(true);
        timeoutSprite.gameObject.SetActive(false);
        MainText.SetActive(true);
        frameIdx.SetActive(true);
        description.gameObject.SetActive(true);
        timeText.gameObject.SetActive(true);
    }

    private void EndGame()
    {
        //Time.timeScale = 0f;
        //endPanel.SetActive(true);
        //scoreText.text = scoreText.text + score;
        SceneManager.LoadScene(3);
    }

    private void AddCard()
    {
        
        for (int x = 0; x < 10; x++)
        {
            AddGrowthCard(GrowthContainer.initiativeId[x], GrowthContainer.description[x], GrowthContainer.imageRaw[x]);
            AddProfitCard(ProfitContainer.initiativeId[x], ProfitContainer.description[x], ProfitContainer.imageRaw[x]);
        }
    }

    private void RemoveCard(string id)
    {
        foreach(Card card in cardList)
        {
            if(card.id == id)
            {
                cardList.Remove(card);
            }
        }
    }
}
