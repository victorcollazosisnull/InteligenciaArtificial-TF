using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [System.Serializable]
    public class PanelData
    {
        public RectTransform panel;
        public Vector3 hiddenPosition;
        public Vector3 visiblePosition = Vector3.zero;
        [HideInInspector] public Vector3 velocity;
        [HideInInspector] public bool isVisible;
    }

    public float smoothTime = 0.3f;
    public PanelData[] panels;

    private void Start()
    {
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].panel.localPosition = panels[i].hiddenPosition;
            panels[i].velocity = Vector3.zero;
            panels[i].isVisible = false;
        }
    }

    private void Update()
    {
        for (int i = 0; i < panels.Length; i++)
        {
            Vector3 targetPosition = panels[i].isVisible ? panels[i].visiblePosition : panels[i].hiddenPosition;
            panels[i].panel.localPosition = Vector3.SmoothDamp(panels[i].panel.localPosition, targetPosition, ref panels[i].velocity, smoothTime, Mathf.Infinity, Time.unscaledDeltaTime);
        }
    }

    public void TogglePanel(int index)
    {
        if (index < 0 || index >= panels.Length)
        {
            Debug.LogWarning("no se puede");
            return;
        }

        panels[index].isVisible = !panels[index].isVisible;
    }

    public void ShowPanel(int index)
    {
        if (index >= 0 && index < panels.Length)
        {
            panels[index].isVisible = true;
        }
    }

    public void HidePanel(int index)
    {
        if (index >= 0 && index < panels.Length)
        {
            panels[index].isVisible = false;
        }
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void Play()
    {
        SceneManager.LoadScene("SampleScene");
    }
}