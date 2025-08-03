
using UnityEngine;
using UnityEngine.UI;

public class PetDisplay : MonoBehaviour
{
    [HideInInspector] public GameObject petItemPrefab;
    [HideInInspector] public Transform petListContainer;

    public void DisplayPets(Pet[] pets)
    {
        foreach (Transform child in petListContainer)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < pets.Length; i++)
        {
            var pet = pets[i];
            var petRow = Instantiate(petItemPrefab, petListContainer);

            // Find table cells
            var idText = petRow.transform.Find("IdCell").GetComponent<Text>();
            var nameText = petRow.transform.Find("NameCell").GetComponent<Text>();
            var createdText = petRow.transform.Find("CreatedCell").GetComponent<Text>();
            var updatedText = petRow.transform.Find("UpdatedCell").GetComponent<Text>();

            // Populate table cells
            idText.text = pet.id.ToString();
            nameText.text = pet.name;
            createdText.text = pet.createdAt.ToLocalTime().ToString("yyyy-MM-dd HH:mm");
            updatedText.text = pet.updatedAt.ToLocalTime().ToString("yyyy-MM-dd HH:mm");

            // Alternate row colors for better readability
            var rowImage = petRow.GetComponent<Image>();
            if (i % 2 == 1)
            {
                rowImage.color = new Color(0.94f, 0.94f, 0.94f, 1f); // Slightly darker for alternate rows
            }

            petRow.SetActive(true);
        }
    }
}
