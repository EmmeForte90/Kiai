using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitySlotManager : MonoBehaviour
{
    [SerializeField] private int slotId;
    private Skill assignedSkill;
public static AbilitySlotManager instance;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }}
    // Metodo per assegnare una nuova abilità allo slot
    public void AssignSkill(Skill newSkill)
    {
        assignedSkill = newSkill;
        // Inserisci qui la logica per aggiornare l'interfaccia grafica dello slot con i dati della nuova abilità
    }

    // Metodo per rimuovere l'abilità dallo slot
    public void RemoveSkill()
    {
        assignedSkill = null;
        // Inserisci qui la logica per rimuovere l'interfaccia grafica dell'abilità dallo slot
    }

    // Metodo per ottenere l'ID dello slot
    public int GetSlotId()
    {
        return slotId;
    }

    // Metodo per ottenere l'abilità assegnata allo slot
    public Skill GetAssignedSkill()
    {
        return assignedSkill;
    }
}
