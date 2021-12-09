using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class InteractionCollectable : MonoBehaviour
{
   public InteractionDetector interaction;
   public ItemCollectionDescription collectableItems;
   private PlayerController player;
   private MessageController message;
   private AudioController audio;

   public Action<InteractionCollectable> OnRemove;
   private void Start()
   {
      player = WorldGraph.Retrieve(typeof(PlayerController)) as PlayerController;
      message = WorldGraph.Retrieve(typeof(MessageController)) as MessageController;
      audio = WorldGraph.Retrieve(typeof(AudioController)) as AudioController;
      interaction.OnInteraction -= OnCollect;
      interaction.OnInteraction += OnCollect;
   }

   private void OnCollect(int force, PlayerToolActionType type)
   {
      var chosen = RaritySelector.GetRandom(collectableItems.collection.descriptions.ToList<IRandomProbability>()) as ItemDescription;

      if (!player.stateController.HasCarrySpace(chosen.item.weight))
      {
         message.QueMessage("Not enough space to carry");
         return;
      }

      message.QueMessage($"Found {chosen.item.menuName}");
      audio.PlaySound(AudioController.AudioClipName.CollectItem);
      player.Inventory.AddByDescription(chosen);
      interaction.OnInteraction -= OnCollect;
      OnRemove?.Invoke(this);
   }
   
}
