using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MessageController : MonoBehaviour
{
   public GameObject messageBlock;
   public Text text;

   private void Awake()
   {
      WorldGraph.Subscribe(this, typeof(MessageController));
   }

   public void QueMessage(string message)
   {
      messageBlock.SetActive(true);
      text.text = message;
      StartCoroutine(NextMessage());
   }

   private IEnumerator NextMessage()
   {
      yield return new WaitForSeconds(2f);
      messageBlock.SetActive(false);
   }
}
