using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaSystem : SingletonMono<ManaSystem>
{
    [SerializeField] private ManaUI manaUI;

    private const int MAX_MANA = 3;

    private int currentMana = MAX_MANA;

    void Start()
    {
        manaUI.UpdateManaText(currentMana);
    }

    private void OnEnable()
    {
        ActionSystem.AttachPerformer<SpendManaGA>(SpendManaPerformer);
        ActionSystem.AttachPerformer<RefillManaGA>(RefillManaPerformer);
        ActionSystem.SubscribeReaction<EnermyTurnGA>(EnermyEndTurnPerformer, ReactionTiming.POST);
    }

    private void OnDisable()
    {
        ActionSystem.DetachPerformer<SpendManaGA>();
        ActionSystem.DetachPerformer<RefillManaGA>();
        ActionSystem.UnsubscribeReaction<EnermyTurnGA>(EnermyEndTurnPerformer, ReactionTiming.POST);
    }

    public bool HasEnoughMana(int manaCost)
    {
        return currentMana >= manaCost;
    }


    private IEnumerator SpendManaPerformer(SpendManaGA ga)
    {
        currentMana -= ga.ManaCost;
        manaUI.UpdateManaText(currentMana);
        yield return null;
    }

    private IEnumerator RefillManaPerformer(RefillManaGA refill)
    {
        currentMana = MAX_MANA;
        manaUI.UpdateManaText(currentMana);
        yield return null;
    }

    private void EnermyEndTurnPerformer(EnermyTurnGA enermyTurnGA)
    {
        RefillManaGA refillManaGA = new RefillManaGA();
        ActionSystem.Instance.AddReaction(refillManaGA);
    }
}
