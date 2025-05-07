namespace NEC.PoolModule
{
    public interface IPooled
    {
        void OnGet();
        void OnRelease();
    }
}
