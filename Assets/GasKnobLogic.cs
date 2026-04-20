using UnityEngine;

public class GasKnobLogic : MonoBehaviour
{
    private HingeJoint hinge;
    
    [Header("Настройки газа")]
    public bool isGasOn = false; // Переменная, которую будут проверять спичка и пламя
    public float thresholdAngle = -20f; // Угол, после которого считаем, что газ пошел

    void Start()
    {
        // Находим компонент шарнира на этой же ручке
        hinge = GetComponent<HingeJoint>();
    }

    void Update()
    {
        // Проверяем текущий угол шарнира
        float currentAngle = hinge.angle;

        // Если ручка повернута дальше порога (в минус, так как крутим влево к High)
        if (currentAngle < thresholdAngle)
        {
            if (!isGasOn)
            {
                isGasOn = true;
                Debug.Log("ГАЗ ОТКРЫТ: Теперь можно подносить спичку!");
            }
        }
        else
        {
            if (isGasOn)
            {
                isGasOn = false;
                Debug.Log("ГАЗ ВЫКЛЮЧЕН");
            }
        }
    }
}