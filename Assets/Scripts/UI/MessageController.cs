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

   public GameObject answerPrefab;
   public Transform answerContainer;
   
   private List<MessageQuestion> messageQue = new List<MessageQuestion>();
   
   private Coroutine waitTimer;
   private InputController inputController;
   private void Awake()
   {
      WorldGraph.Subscribe(this, typeof(MessageController));
   }

   private void Start()
   {
      inputController = WorldGraph.Retrieve(typeof(InputController)) as InputController;
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
         
         foreach (Transform child in answerContainer)
         {
            Destroy(child.gameObject);
         }
         
         if (messageQue[0].answers.Count >= 1)
         {
            for(var i=0;i<messageQue[0].answers.Count;i++)
            {
               Text answerText = Instantiate(answerPrefab, answerContainer).GetComponent<Text>();
               answerText.text = $"({i+1}){messageQue[0].answers[i].Item1}";
            }
            inputController.ChangeScheme("MessageAnswer");
            inputController.ApplyMessageBoxAnswer += OnAnswer;
            return;
         }
         
         waitTimer = StartCoroutine(WaitForNextMessage());
      }
   }

   private IEnumerator WaitForNextMessage()
   {
      yield return new WaitForSeconds(2f);
      EndMessage();
      yield return new WaitForSeconds(.2f);
      waitTimer = null;
      if (messageQue.Count > 0) ShowMessage();
   }

   private void OnAnswer(int id)
   {
      inputController.ApplyMessageBoxAnswer -= OnAnswer;
      var storedAnswer = messageQue[0].answers[id - 1];
      EndMessage();
      inputController.ChangeScheme("Player");
      storedAnswer.Item2?.Invoke();
   }

   private void EndMessage()
   {
      messageBlock.SetActive(false);
      messageQue.RemoveAt(0);
   }
}
