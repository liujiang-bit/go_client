namespace Game
{
    public interface IBaseWorldEffectMediator
    {
        void UpdateEffects(MapObjectInfoEntity objectInfoEntity);
        void DisposeEffect();
    }

}
