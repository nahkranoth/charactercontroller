using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class InteractionCollectable : MonoBehaviour
{
   public InteractionDetector interaction;
   public ItemCollectionDescription collectableItems;
   private PlayerController player;
   private MessageController message;

   public Action<InteractionCollectable> OnRemove;
   private void Start()
   {
      player = WorldGraph.Retrieve(typeof(PlayerController)) as PlayerController;
      message = WorldGraph.Retrieve(typeof(MessageController)) as MessageController;
      StartCoroutine(StartCollectDetection());
   }

   IEnumerator StartCollectDetection()
   {
      yield return new WaitForSeconds(2f);
      interaction.OnInteraction -= OnCollect;
      interaction.OnInteraction += OnCollect;
   }

   private void OnCollect(int force)
   {
      var randI = Random.Range(0, collectableItems.collection.descriptions.Count);
      var chosen = collectableItems.collection.descriptions[randI];
      message.QueMessage($"Found {chosen.item.menuName}");
      player.inventory.AddByDescription(chosen);
      interaction.OnInteraction -= OnCollect;
      OnRemove?.Invoke(this);
   }
   
}
