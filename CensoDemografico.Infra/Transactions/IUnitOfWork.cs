namespace CensoDemografico.Infra.Transactions
{
    public interface IUnitOfWork
    {
        void Commit();
    }
}
