using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class MenuWheelController : MonoBehaviour
{
    public float growFactor = 12f;
    public float radius = 22f;
    public float rotateSpeed = 10f;
    public Transform wheelHolder;
    public GameObject wheelItemPrefab;
    public TextMeshProUGUI selectedItemTxt;
    public Vector3 offset;
    
    private float degreeFract;
    private InputController inputController;
    private WorldController worldController;
    private PlayerController playerController;
    private ItemBehaviourController itemBehaviourController;

    private List<MenuWheelItem> itemList;
    private int spinStep = 0;
    private float spinCounter = 0;
    private int selectionStep = 0;

    private Vector2 direction;
    private bool spinning = false;
    private bool firstRotationPart = true;

    private List<Item> currentItems;
    
    private void Start()
    {
        inputController = WorldGraph.Retrieve(typeof(InputController)) as InputController;
        worldController = WorldGraph.Retrieve(typeof(WorldController)) as WorldController;
        playerController = WorldGraph.Retrieve(typeof(PlayerController)) as PlayerController;

        Hide();
        
        itemBehaviourController = WorldGraph.Retrieve(typeof(ItemBehaviourController)) as ItemBehaviourController;
        
        worldController.OnToggleMenu -= SetMyState;
        worldController.OnToggleMenu += SetMyState;
    }

    private void Show()
    {
        currentItems = playerController.Inventory.storage;
        wheelHolder.gameObject.SetActive(true);
        selectedItemTxt.gameObject.SetActive(true);
    }

    public void Select()
    {
        if (currentItems.Count == 0) return;
        var cItem = itemList[selectionStep].currentItem;
        itemBehaviourController.Execute(cItem);
        Debug.Log($"Take Item {cItem.menuName}");
        if (cItem.consumable && playerController.Inventory.TakeItem(cItem))
        {
            currentItems = playerController.Inventory.storage;
            spinStep = 0;
            selectionStep = 0;
            MakeWheel();
        };
        SetText();
    }

    public void SetMyState(bool state)
    {
        if (state)
        {
            Initialize();
            return;
        }
        Hide();
    }

    public void Initialize()
    {
        Show();
        inputController.Directions -= OnDirections;
        inputController.Directions += OnDirections;
        inputController.ApplyTool -= Select;
        inputController.ApplyTool += Select;
        spinStep = 0;
        selectionStep = 0;
        
        MakeWheel();
    }
    
    public void Hide()
    {
        wheelHolder.gameObject.SetActive(false);
        selectedItemTxt.gameObject.SetActive(false);
        inputController.Directions -= OnDirections;
        inputController.ApplyTool -= Select;
    }

    private void SetSpinStep(int dir)
    {
        spinStep = spinStep+dir;
        spinStep = Helpers.CycleConstraint(currentItems.Count-1, 0, spinStep);
    }
    
    private void SetSelectionStep(int dir){
        selectionStep = selectionStep-dir;
        selectionStep = Helpers.CycleConstraint(currentItems.Count-1, 0, selectionStep);
        SetText();
    }

    private void SetText()
    {
        selectedItemTxt.text = $"{currentItems[selectionStep].amount}x {currentItems[selectionStep].menuName}";
    }
    
    private void OnDirections(Vector2 _direction)
    {
        if (spinning || _direction.x == 0 || currentItems.Count <= 0) return;
        direction = -_direction;
        spinCounter = 0;
        SetSpinStep((int)direction.x);
        SetSelectionStep((int)direction.x);
        spinning = true;
    }

    private void Update()
    {
        if (spinning)
        {
           for(int i=0;i<itemList.Count;i++) {
               var item = itemList[i];
               item.currentAngle += Time.deltaTime * rotateSpeed * direction.x;
               item.transform.localPosition = GetPosition(item.currentAngle);
           }
           spinCounter += Time.deltaTime * rotateSpeed;
           if (spinCounter >= degreeFract)
           {
               for(int i=0;i<itemList.Count;i++) {
                   var item = itemList[i];
                   item.currentAngle = Mathf.Round(item.localStartAngle + (spinStep * degreeFract));
                   item.transform.localPosition = GetPosition(item.currentAngle);
               }
               spinning = false;
           }
        }
        transform.position = Camera.main.WorldToScreenPoint(playerController.transform.position) + offset;
    }

    private void MakeWheel()
    {
        foreach (var wheelItem in wheelHolder.GetComponentsInChildren<MenuWheelItem>())
        {
            Destroy(wheelItem.gameObject);
        }
        itemList = new List<MenuWheelItem>();
        degreeFract = Mathf.Round(360f/currentItems.Count);
        for (int i = 0; i < currentItems.Count; i++)
        {
            var wheelItem = Instantiate(wheelItemPrefab, wheelHolder);
            var angle = i * degreeFract;
            wheelItem.transform.localPosition = GetPosition(angle);
            var wm = wheelItem.GetComponent<MenuWheelItem>();
            wm.localStartAngle = angle;
            wm.currentAngle = angle;
            wm.img.sprite = currentItems[i].menuSprite;
            wm.currentItem = currentItems[i];
            itemList.Add(wm);
        }
    }
    
    private Vector3 GetPosition(float angle)
    {
        var rot = Quaternion.AngleAxis(angle,Vector3.forward);
        var radiusOffset = Mathf.Max(0, currentItems.Count - 7) * growFactor;
        return (rot * Vector3.up) * (radius + radiusOffset);
    }
    
}
