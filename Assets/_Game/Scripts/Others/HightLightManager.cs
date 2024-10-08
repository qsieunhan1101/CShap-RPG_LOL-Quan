using UnityEngine;
using UnityEngine.EventSystems;

public class HightLightManager : MonoBehaviour
{
    [SerializeField] private Player player;
    private Transform highlightedObj;
    private Transform selectedObj;

    public LayerMask selectableLayer;

    private Outline highlightOutline;
    private RaycastHit hit;

    private void Start()
    {
        player = GetComponent<Player>();
    }
    void Update()
    {
        if (player.IsMobileMode == false)
        {
            HoverHighLight();

        }
    }

    public void HoverHighLight()
    {
        if (highlightedObj != null)
        {
            highlightOutline.enabled = false;
            highlightedObj = null;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out hit, selectableLayer))
        {
            highlightedObj = hit.transform;
            if (highlightedObj.CompareTag("Enemy") && highlightedObj != selectedObj)
            {
                highlightOutline = highlightedObj.GetComponent<Outline>();
                highlightOutline.enabled = true;
            }
            else
            {
                highlightedObj = null;
            }
        }
    }

    public void SelectedHighLight()
    {
        if (highlightedObj.CompareTag("Enemy"))
        {
            if (selectedObj != null)
            {
                selectedObj.GetComponent<Outline>().enabled = false;
            }

            selectedObj = hit.transform;
            selectedObj.GetComponent<Outline>().enabled = true;

            highlightOutline.enabled = true;
            highlightedObj = null;
        }
    }
    public void DeselectHighLight()
    {
        if (selectedObj != null)
        {
            selectedObj.GetComponent<Outline>().enabled = false;
            selectedObj = null;
        }
    }
}
