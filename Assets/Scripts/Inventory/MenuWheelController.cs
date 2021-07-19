using System.Collections.Generic;
using Inventory;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class MenuWheelController : MonoBehaviour
{
    public int amount = 7;
    public float growFactor = 12f;
    public float radius = 22f;
    public float rotateSpeed = 10f;
    public GameObject wheelItemPrefab;
    private float degreeFract;
    private InputController inputController;

    private List<MenuWheelItem> itemList;
    private MenuWheelItem currentItem;
    private int spinStep = 0;
    private int spinTarget;
    private float spinCounter = 0;
    
    private Vector2 direction;
    private bool spinning = false;
    private bool firstRotationPart = true;
    private void Start()
    {
        itemList = new List<MenuWheelItem>();
        inputController = WorldGraph.Retrieve(typeof(InputController)) as InputController;
        inputController.Directions -= OnDirections;
        inputController.Directions += OnDirections;
        MakeWheel();
    }

    private void OnDirections(Vector2 _direction)
    {
        if (spinning || _direction.x == 0) return;
        direction = _direction;
        spinCounter = 0;
        spinStep = spinStep+(int)direction.x;
        spinStep = Helpers.CycleConstraint(amount-1, 0, spinStep);
        currentItem = itemList[spinStep];
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
                    item.transform.localPosition = GetPosition(item.localStartAngle+(spinStep*degreeFract));
                }
                spinning = false;
            }
        }
    }

    private void MakeWheel()
    {
        degreeFract = Mathf.Round(360f/amount);
        for (int i = 0; i < amount; i++)
        {
            var wheelItem = Instantiate(wheelItemPrefab, transform);
            var angle = i * degreeFract;
            wheelItem.transform.localPosition = GetPosition(angle);
            var wm = wheelItem.GetComponent<MenuWheelItem>();
            wm.localStartAngle = angle;
            wm.currentAngle = angle;
            itemList.Add(wm);
        }
    }
    
    private Vector3 GetPosition(float angle)
    {
        var rot = Quaternion.AngleAxis(angle,Vector3.forward);
        var radiusOffset = Mathf.Max(0, amount - 7) * growFactor;
        return (rot * Vector3.up) * (radius + radiusOffset);
    }
    
}
