using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MessageController : MonoBehaviour
{
   public GameObject messageBlock;
   public Text questionText;
   public Text answerOneText;
   public Text answerTwoText;
   private List<MessageQuestion> messageQue = new List<MessageQuestion>();
   
   private Coroutine waitTimer;

   private void Awake()
   {
      WorldGraph.Subscribe(this, typeof(MessageController));
   }
   
   public void QueMessageQuestion(string message, List<(string, Action)> answers)
   {
      messageQue.Add(new MessageQuestion{question = message, answers = answers});
      ShowMessage();
   }

   public void QueMessage(string message)
   {
      messageQue.Add(new MessageQuestion{question = message});
      ShowMessage();
   }

   private void ShowMessage()
   {
      if(waitTimer == null && messageQue.Count > 0)
      {
         questionText.text = messageQue[0].question;
         messageBlock.SetActive(true);
         answerOneText.text = "";
         answerTwoText.text = "";
         if (messageQue[0].answers.Count >= 2)
         {
            answerOneText.text = messageQue[0].answers[0].Item1;
            answerTwoText.text = messageQue[0].answers[1].Item1;
         }
         
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
