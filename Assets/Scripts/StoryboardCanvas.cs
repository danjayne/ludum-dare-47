using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class StoryboardCanvas : MonoBehaviour
{
    public RectTransform FirstPanel;
    public RectTransform CurrentPanel;

    private void Start()
    {
        FirstPanel.gameObject.SetActive(true);
        CurrentPanel = FirstPanel;
    }

    public void NextScene()
    {
        var i = CurrentPanel.name.Split(new string[] { "Board" }, System.StringSplitOptions.RemoveEmptyEntries)[0];
        var nextPanel = gameObject.transform.Find($"Board{int.Parse(i) + 1}");

        if (nextPanel == null)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            return;
        }

        CurrentPanel.gameObject.SetActive(false);        
        CurrentPanel = nextPanel.GetComponent<RectTransform>();
        CurrentPanel.gameObject.SetActive(true);
    }
}
