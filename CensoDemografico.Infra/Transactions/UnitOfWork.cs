using CensoDemografico.Infra.Context;

namespace CensoDemografico.Infra.Transactions
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CensoDemograficoContext _context;

        public UnitOfWork(CensoDemograficoContext context)
        {
            _context = context;
        }

        public void Commit()
        {
            _context.SaveChanges();
        }
    }
}
