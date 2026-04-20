using UnityEngine;

public class BurnerIgnition : MonoBehaviour
{
    [Header("Ссылки")]
    public GasKnobLogic knob; 

    public DiagnosticManager manager; 
    public int burnerIndex;
    
    [Header("Нормальное состояние")]
    public ParticleSystem blueFlame; 

    [Header("Аварийное состояние (Ошибка)")]
    [Tooltip("Поставь галочку, если эта конфорка должна работать с ошибкой")]
    public bool isFaulty = false; 
    public ParticleSystem faultyFlame; // Система частиц для "плохого" огня
    public AudioSource poppingSound;   // Звук потрескивания/хлопков

    private void Start()
    {
        // В начале все эффекты выключены
        if (blueFlame != null) blueFlame.Stop();
        if (faultyFlame != null) faultyFlame.Stop();
        if (poppingSound != null) poppingSound.Stop();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Match"))
        {
            if (knob != null && knob.isGasOn)
            {
                Ignite();
            }
        }
    }

    void Ignite()
    {
        // Выбираем, какой сценарий запустить
        if (isFaulty)
        {
            // Сценарий ОШИБКИ
            if (faultyFlame != null && !faultyFlame.isPlaying)
            {
                faultyFlame.Play();
                if (poppingSound != null) poppingSound.Play(); // Включаем хлопки
                Debug.Log("ВНИМАНИЕ: Конфорка зажглась с ОШИБКОЙ!");
            }
            // Гарантируем, что синий огонь выключен
            if (blueFlame != null) blueFlame.Stop();
        }
        else
        {
            // Сценарий НОРМА
            if (blueFlame != null && !blueFlame.isPlaying)
            {
                blueFlame.Play();
                Debug.Log("Конфорка зажглась успешно.");
            }
            // Гарантируем, что аварийные эффекты выключены
            if (faultyFlame != null) faultyFlame.Stop();
            if (poppingSound != null) poppingSound.Stop();
        }

       if(manager != null) 
        {
            manager.MarkBurnerAsIgnited(burnerIndex);
        }        
    }

    void Update()
    {
        // Логика гашения пламени (одинаковая для обоих сценариев)
        if (knob != null && !knob.isGasOn)
        {
            if (isFaulty)
            {
                if (faultyFlame != null && faultyFlame.isPlaying) faultyFlame.Stop();
                if (poppingSound != null && poppingSound.isPlaying) poppingSound.Stop();
            }
            else
            {
                if (blueFlame != null && blueFlame.isPlaying) blueFlame.Stop();
            }
        }
    }
}