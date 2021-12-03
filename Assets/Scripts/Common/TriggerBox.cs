using System;
using UnityEngine;

public class TriggerBox : MonoBehaviour
{

  public Action<Collider2D> OnTriggerStay;
  public Action<Collider2D> OnTriggerExit;

  private void OnTriggerStay2D(Collider2D other)
  {
    OnTriggerStay?.Invoke(other);
  }
  
  private void OnTriggerExit2D(Collider2D other)
  {
    OnTriggerExit?.Invoke(other);
  }
  
  
}
