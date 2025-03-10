using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text1;
    [SerializeField] private TextMeshProUGUI text2;
    [SerializeField] private TextMeshProUGUI text3;

    private List<TextMeshProUGUI> textList = new List<TextMeshProUGUI>();
    private int currentIndex = 0;

    private void Start()
    {
        textList.Add(text1);
        textList.Add(text2);
        textList.Add(text3);

        textList[0].gameObject.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && currentIndex < textList.Count)
        {
            textList[currentIndex].gameObject.SetActive(false);
            currentIndex++;

            if (currentIndex < textList.Count)
            {
                textList[currentIndex].gameObject.SetActive(true);
            }

            if (currentIndex == 2)
            {
                TutorialGameplay.Instance.canSpawn = true;
                TutorialGameplay.Instance.SpawnEnemy();
            }

            if (currentIndex == textList.Count)
            {
                TutorialGameplay.Instance.tutorialCompleted = true;
            }
        }
    }
}
