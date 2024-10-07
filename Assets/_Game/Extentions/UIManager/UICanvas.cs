using UnityEngine;

public class UICanvas : MonoBehaviour
{
    [SerializeField] bool destroyOnclose = false;

    private void Awake()
    {
        //su ly tai tho
        RectTransform rect = GetComponent<RectTransform>();
        float ratio = (float)Screen.width / (float)Screen.height;
        if (ratio > 2.1f)
        {
            Vector2 leftBottom = rect.offsetMin;
            Vector2 rightTop = rect.offsetMax;

            leftBottom.y = 0f;
            rightTop.y = -100f;

            rect.offsetMin = leftBottom;
            rect.offsetMax = rightTop;
        }
    }
    //goi truoc khi duoc Active
    public virtual void SetUp()
    {

    }
    //goi sau khi duoc Active
    public virtual void Open()
    {
        gameObject.SetActive(true);
    }
    //tat cavas sau time
    public virtual void Close(float time)
    {
        Invoke(nameof(CloseDirectly), time);
    }
    //tat canvas truc tiep
    public virtual void CloseDirectly()
    {
        if (destroyOnclose)
        {
            Destroy(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
