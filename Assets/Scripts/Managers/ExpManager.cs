using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ExpManager : MonoBehaviour
{
    public static ExpManager Instance;

    [SerializeField] private Image _fillImage;
    [SerializeField] private Text _levelText;
    [SerializeField] private Text _expText;
    [SerializeField] private Text _requiredExpText;

    private int _currentLevel;
    private int _expToNextLevel;
    private float _displayedExp;
    private float _currentExp;

    public static int Combo;

    private Coroutine _expRoutine;

    public GameObject LevelUpPanel;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _currentLevel = 1;
        _expToNextLevel = 800;

        _displayedExp = 0f;
        _currentExp = 0f;

        Combo = 1;
    }

    public void AddExp(float amount)
    {
        _currentExp += amount;

        if (_expRoutine != null)
            StopCoroutine(_expRoutine);

        _expRoutine = StartCoroutine(AnimateExpBar());
    }

    private void LevelUp()
    {
        GridManager.Instance.ClearBoard();

        _currentLevel++;
        _expToNextLevel = (int)(_expToNextLevel * 1.25f);
        _displayedExp = 0f;
        _currentExp = 0f;

        Combo = 1;

        LevelUpPanel.SetActive(true);
        UpdateUI();
    }

    private void UpdateUI()
    {
        _fillImage.fillAmount = _currentExp / _expToNextLevel;
        _levelText.text = "Level " + _currentLevel;
        _requiredExpText.text = _expToNextLevel.ToString();
        _expText.text = _currentExp.ToString();
    }


    private IEnumerator AnimateExpBar()
    {
        float duration = 1f;

        float startExp = _displayedExp;
        float endExp = _currentExp;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            float easedT = Mathf.SmoothStep(0, 1, t);

            _displayedExp = Mathf.Lerp(startExp, endExp, easedT);
            _fillImage.fillAmount = _displayedExp / _expToNextLevel;
            _expText.text = Mathf.FloorToInt(_displayedExp).ToString();

            yield return null;
        }

        _displayedExp = _currentExp;
        _fillImage.fillAmount = _displayedExp / _expToNextLevel;
        _expText.text = Mathf.FloorToInt(_displayedExp).ToString();

        if (_currentExp >= _expToNextLevel)
            LevelUp();
    }

    public void ContinueButton()
    {
        LevelUpPanel.SetActive(false);
    }
}
