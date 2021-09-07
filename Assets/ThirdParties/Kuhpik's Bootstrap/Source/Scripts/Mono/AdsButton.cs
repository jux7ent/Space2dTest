using System.Collections;
using System.Collections.Generic;
using Kuhpik;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class AdsButton : MonoBehaviour {
    [SerializeField] private bool setupManually = false;

    [SerializeField] [ShowIf("setupManually")] private Button button;
    [SerializeField] [ShowIf("setupManually")] private Image image;
    
    private Color startColor;
    private GameObject loading;

    private float lastCheckTime = 0f;

    private void Awake() {
        if (button == null) {
            button = GetComponent<Button>();
            image = GetComponent<Image>();
        }

        startColor = image.color;

        ChangeActiveStatus(false);
    }

    private void Update() {
        if (Time.time > lastCheckTime + 1f) {
            lastCheckTime = Time.time;
            ChangeActiveStatus(CheckAdvertisementAvailability());
        }
    }

    private void ChangeActiveStatus(bool value) {
        button.interactable = value;
        
        if (!value) {
            Color color = Color.grey / 2f;
            color.a = 1f;
            if (image) {
                image.color = color;
            }
            
            if (loading == null) {
                loading = Instantiate(Resources.Load<GameObject>("LoadingIndicator"), transform);
            }
        } else {
            if (image) {
                image.color = startColor;
            }
            
            Destroy(loading);
        }
    }

    private bool CheckAdvertisementAvailability() {
        return false;
        //return Bootstrap.GetSystem<AdvertisementManagerSystem>().CheckRewardedAvailable();
    }
}