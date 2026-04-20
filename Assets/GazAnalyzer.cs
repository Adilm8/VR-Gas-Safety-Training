using UnityEngine;
using TMPro;

public class GasAnalyzer : MonoBehaviour
{
    public TextMeshProUGUI screenText;
    public AudioSource alarmSound;

    [SerializeField] private int activeGasZones = 0; // Счетчик источников
    private float currentPPM = 0f;
    private float targetPPM = 0f;

    void Update()
    {
        bool isDetecting = activeGasZones > 0;

        if (isDetecting) 
        {
            targetPPM = Random.Range(480f, 520f);
            if (alarmSound != null && !alarmSound.isPlaying) alarmSound.Play();
        }
        else 
        {
            targetPPM = 0f;
            if (alarmSound != null && alarmSound.isPlaying) alarmSound.Stop();
        }

        currentPPM = Mathf.Lerp(currentPPM, targetPPM, Time.deltaTime * 2f);
        
        if (screenText != null)
        {
            string status = isDetecting ? "<color=red>УТЕЧКА</color>" : "<color=green>НОРМА</color>";
            screenText.text = $"{status}\n{currentPPM:F0} PPM";
        }
    }

    public void AddGasSource() 
    { 
        activeGasZones++; 
        Debug.Log($"<color=orange>Analyzer:</color> Источник добавлен. Всего зон: {activeGasZones}");
    }

    public void RemoveGasSource() 
    { 
        activeGasZones = Mathf.Max(0, activeGasZones - 1); 
        Debug.Log($"<color=green>Analyzer:</color> Источник убран. Осталось зон: {activeGasZones}");
    }
}