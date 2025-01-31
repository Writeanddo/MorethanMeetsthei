using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using UnityEngine.UIElements;


public class TitleScreenManager : MonoBehaviour
{
    [SerializeField] UIDocument uiDoc;
    [SerializeField] private PlayableDirector timelineDirector;
    [SerializeField] private GameObject starryNightParticleSystem; // to make sure we disable the particle system once we're done with the title screen
    [SerializeField] private GameTimer gameTimer;
    [SerializeField] List<string> textToDisplay;
    [SerializeField] GameObject ufoPrefab;

    private VisualElement rootEl;
    private VisualElement titleTextContainer;
    private VisualElement pressStartTextContainer; // the element where we indicate that the player has to press a button to start
    public event Action<List<string>> AreaEntered;

    private void Awake()
    {
        GetUIElements();
    }

    private void GetUIElements()
    {
        rootEl = uiDoc.rootVisualElement;
        titleTextContainer = rootEl.Q(className: "main-title");
        pressStartTextContainer = rootEl.Q(className: "press-start-text");
    }

    // display Title - disable all controls
    private void Start() {
        rootEl = uiDoc.rootVisualElement;
        titleTextContainer = rootEl.Q(className: "main-title");
        pressStartTextContainer = rootEl.Q(className: "press-start-text");

        DisableCharControls();
        DisableMenuControls();
        StartCoroutine("TitleScreenAppearSequence");
    }

    IEnumerator TitleScreenAppearSequence(){
        yield return new WaitForSeconds(0.5f);
        titleTextContainer.AddToClassList("element--active");
        yield return new WaitForSeconds(2.5f);
        pressStartTextContainer.AddToClassList("element--active");
        InputControl.menuControlEnabled = true;
    }

    void Update(){
        if (InputControl.menuControlEnabled && Input.GetKeyDown(KeyCode.Space)){
            StartCoroutine("StartGameSequence");
        }
    }

    IEnumerator StartGameSequence(){
        InputControl.menuControlEnabled = false;
        titleTextContainer.RemoveFromClassList("element--active");
        pressStartTextContainer.RemoveFromClassList("element--active");
        timelineDirector.Play();
        yield return new WaitForSeconds(1.5f);
        AreaEntered?.Invoke(textToDisplay);
        yield return new WaitForSeconds(3.0f);
        starryNightParticleSystem.SetActive(false);
        InputControl.charControlEnabled = true;
        gameTimer.StartTimer();
        Instantiate(ufoPrefab);
        gameObject.SetActive(false);
    }

    private void DisableCharControls() {
        InputControl.charControlEnabled = false;
    }

    private void DisableMenuControls()
    {
        InputControl.menuControlEnabled = false;
    }

}
