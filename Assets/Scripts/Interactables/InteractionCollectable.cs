using System.Collections;
using UnityEngine;

public class InteractionCollectable : MonoBehaviour
{
   public InteractionDetector interaction;
   public ItemDescription collectableItem;
   private PlayerController player;
   private MessageController message;
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
      message.QueMessage($"Found {collectableItem.item.menuName}");
      player.inventory.AddByDescription(collectableItem);
      interaction.OnInteraction -= OnCollect;
      Destroy(gameObject);
   }
   
}
