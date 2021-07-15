using System;
using UnityEngine;

public class TriggerBox : MonoBehaviour
{

  public Action<Collider2D> OnTriggerStay;

  private void OnTriggerStay2D(Collider2D other)
  {
    OnTriggerStay?.Invoke(other);
  }
}
