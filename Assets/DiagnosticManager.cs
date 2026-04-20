using UnityEngine;
using TMPro;

public class DiagnosticManager : MonoBehaviour
{
    // Статусы для каждой из 2-х конфорок
    public bool[] gasDetected = new bool[2];
    public bool[] ignited = new bool[2];
    
    public TextMeshProUGUI checklistText; 
    public GameObject successPanel;       

    void Start()
    {
        UpdateUI();
        if (successPanel != null) successPanel.SetActive(false);
    }

    // Вызывается из GasLeakZone, когда анализатор в зоне и газ включен
    public void RegisterGasCheck(int index)
    {
        if (index >= 0 && index < gasDetected.Length && !gasDetected[index])
        {
            gasDetected[index] = true;
            UpdateUI();
        }
    }

    // Вызывается из BurnerIgnition при розжиге
    public void MarkBurnerAsIgnited(int index)
    {
        if (index >= 0 && index < ignited.Length && !ignited[index])
        {
            // Зачесть розжиг можно только после проверки газа
            if (gasDetected[index])
            {
                ignited[index] = true;
                UpdateUI();
                CheckFinalSuccess();
            }
        }
    }

    void UpdateUI()
    {
        if (checklistText == null) return;

        checklistText.text = "<color=#FFFF00>ПЛАН ДИАГНОСТИКИ:</color>\n\n" +
                             GetBurnerLine(0, "Конфорка 1 (Слева снизу)") + "\n" +
                             GetBurnerLine(1, "Конфорка 2 (Слева сверху)");
    }

    string GetBurnerLine(int i, string name)
    {
        string step1 = gasDetected[i] ? "<color=green>[V] Газ ок</color>" : "<color=yellow>[ ] Проверь газ</color>";
        string step2 = ignited[i] ? "<color=green>[V] Зажжено</color>" : (gasDetected[i] ? "<color=yellow>[ ] Поджигай</color>" : "[ ] Ожидание");
        
        return $"{name}:\n   {step1} -> {step2}";
    }

    void CheckFinalSuccess()
    {
        if (ignited[0] && ignited[1])
        {
            if (successPanel != null) successPanel.SetActive(true);
            if (checklistText != null) checklistText.gameObject.SetActive(false);
        }
    }
}