using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    Dictionary<System.Type, UICanvas> canvasActives = new Dictionary<System.Type, UICanvas>();
    Dictionary<System.Type, UICanvas> canvasPrefabs = new Dictionary<System.Type, UICanvas>();

    [SerializeField] Transform parent;

    private void Awake()
    {
        UICanvas[] prefab = Resources.LoadAll<UICanvas>("UI/");
        for (int i = 0; i< prefab.Length; i++)
        {
            canvasPrefabs.Add(prefab[i].GetType(), prefab[i]);
        }
    }
    //mo canvas
    public T OpenUI<T>() where T : UICanvas
    {
        T canvas = GetUI<T>();

        canvas.SetUp();
        canvas.Open();
        return canvas;
    }
    //dong cavas sau time
    public void CloseUI<T>(float time) where T : UICanvas
    {
        if (IsUILoaded<T>())
        {
            canvasActives[typeof(T)].Close(time);
        }
    }
    //dong canvas truc tiep
    public void CloseUIDirectly<T>() where T : UICanvas
    {
        if (IsUILoaded<T>())
        {
            canvasActives[typeof(T)].CloseDirectly();
        }

    }
    //kiem tra xem canvas da duoc tao chua
    public bool IsUILoaded<T>() where T : UICanvas
    {
        return canvasActives.ContainsKey(typeof(T)) && canvasActives[typeof(T)] != null;
    }
    //kiem tra canvas da duoc Active hay chua
    public bool IsUIOpened<T>() where T : UICanvas
    {
        return IsUILoaded<T>() && canvasActives[typeof(T)].gameObject.activeSelf;
    }
    //lay canvas
    public T GetUI<T>() where T : UICanvas
    {
        if (!IsUILoaded<T>())
        {
            T prefab = GetUIPrefab<T>();
            T canvas = Instantiate(prefab, parent);
            canvasActives[typeof(T)] = canvas;
        }
        return canvasActives[typeof(T)] as T;
    }
    public T GetUIPrefab<T>() where T : UICanvas
    {
        return canvasPrefabs[typeof(T)] as T;
    }

    //dong tat ca
    public void CloseAll()
    {
        foreach (var canvas in canvasActives)
        {
            if (canvas.Value != null && canvas.Value.gameObject.activeSelf)
            {
                canvas.Value.Close(0);
            }
        }
    }
}
