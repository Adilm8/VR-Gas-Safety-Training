using UnityEngine;

public class GasLeakZone : MonoBehaviour
{
    public GasKnobLogic knob;
    public int burnerIndex;
    
    private bool hasContributed = false; // Флаг: подала ли эта зона сигнал на прибор
    private GasAnalyzer currentAnalyzer;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("AnalyzerSensor"))
        {
            currentAnalyzer = other.GetComponentInParent<GasAnalyzer>();
            if (currentAnalyzer == null) return;

            // ЕСЛИ ГАЗ ВКЛЮЧЕН, А МЫ ЕЩЕ НЕ ГОВОРИЛИ ОБ ЭТОМ ПРИБОРУ
            if (knob.isGasOn && !hasContributed)
            {
                currentAnalyzer.AddGasSource();
                hasContributed = true;
                Debug.Log($"[Burner {burnerIndex}] Газ ВКЛ в зоне. Прибор начал пищать.");

                // Регистрируем проверку в менеджере
                var manager = Object.FindFirstObjectByType<DiagnosticManager>();
                if (manager != null) manager.RegisterGasCheck(burnerIndex);
            }
            // ЕСЛИ ГАЗ ВЫКЛЮЧИЛИ РУЧКОЙ, ПОКА МЫ В ЗОНЕ
            else if (!knob.isGasOn && hasContributed)
            {
                currentAnalyzer.RemoveGasSource();
                hasContributed = false;
                Debug.Log($"[Burner {burnerIndex}] Газ ВЫКЛ ручкой. Прибор замолчал.");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("AnalyzerSensor") && hasContributed)
        {
            if (currentAnalyzer != null)
            {
                currentAnalyzer.RemoveGasSource();
                hasContributed = false;
                Debug.Log($"[Burner {burnerIndex}] Датчик покинул зону. Прибор замолчал.");
            }
        }
    }
}