public class HookEffectController : EffectController
{
    private enum EffectName
    {
        LastHeavyAttackSmoke,
        JumpSmoke,
    }

    public void EnableLastHeavyAttackSmoke() => _effects[(int)EffectName.LastHeavyAttackSmoke].SetActive(true);
    public void EnableJumpSmoke() => _effects[(int)EffectName.JumpSmoke].SetActive(true);
}
