namespace Assets.Scripts.Modes
{
    public interface ICurrentModeManager
    {
        IMode GetCurrentMod();
        void ChangeCurrentMode(IMode newMode);
    }
}