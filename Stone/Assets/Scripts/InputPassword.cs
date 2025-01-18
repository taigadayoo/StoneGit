using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InputPassword : MonoBehaviour
{
    public GameObject BackButton;
    [SerializeField] private TMP_InputField passwordInputField;
    [SerializeField] private Button matchButton;
    [SerializeField] private MatchmakingManager matchManager;

    private void Start()
    {

        matchButton.onClick.AddListener(OnMatchButtonClicked);
    }

    private void OnMatchButtonClicked()
    {
      
        string password = passwordInputField.text.Trim();

        if (!string.IsNullOrEmpty(password))
        {
            matchManager.StartMatching(password);
            BackButton.SetActive(false);
            matchButton.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("パスワードが空です");
        }
    }
}