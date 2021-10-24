using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MessageController : MonoBehaviour
{
   public GameObject messageBlock;
   public Text text;
   private List<string> messageQue = new List<string>();

   private Coroutine waitTimer;

   private void Awake()
   {
      WorldGraph.Subscribe(this, typeof(MessageController));
   }

   public void QueMessage(string message)
   {
      messageQue.Add(message);
      ShowMessage();
   }

   private void ShowMessage()
   {
      if(waitTimer == null)
      {
         text.text = messageQue[0];
         messageBlock.SetActive(true);
         waitTimer = StartCoroutine(WaitForNextMessage());
      }
   }

   private IEnumerator WaitForNextMessage()
   {
      yield return new WaitForSeconds(2f);
      messageBlock.SetActive(false);
      messageQue.RemoveAt(0);
      yield return new WaitForSeconds(.2f);
      waitTimer = null;
      if (messageQue.Count > 0) ShowMessage();
   }
}
