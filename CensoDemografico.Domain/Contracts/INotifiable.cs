using Flunt.Notifications;
using System.Collections.Generic;

namespace CensoDemografico.Domain.Contracts
{
    public interface INotifiable
    {
        IReadOnlyCollection<Notification> Notifications { get; }
        void AddNotification(string property, string message);
    }
}
