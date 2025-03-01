using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;


public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI buttonText; // Référence au texte du bouton
    private Color originalColor;

    private void Start()
    {
        if (buttonText == null)
            buttonText = GetComponentInChildren<TextMeshProUGUI>(); // Récupère le texte si non assigné

        originalColor = buttonText.color; // Sauvegarde la couleur d'origine
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonText.color = Color.white; // Change la couleur en blanc au survol
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonText.color = originalColor; // Remet la couleur d'origine en sortie de survol
    }
}
