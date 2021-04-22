using System.Linq;

using BookingService.DAL;

namespace BookingService.Extensions
{
    /// <summary>
    /// Extension methods for the Appointment Entity
    /// </summary>
    public static class AppointmentExtensions
    {
        public static IQueryable<Appointment> Filter(this IQueryable<Appointment> query, string key, string filter)
        {
            switch (key.ToLower())
            {
                case "name" :
                    query = query.Where(a => a.Name.ToLower() == $"{filter.ToLower()}");
                    break;
                case "animal":
                    query = query.Where(a => a.Animal.ToString() == $"{filter.ToLower()}");
                    break;
                case "canceled":
                    query = query.Where(a => a.Canceled);
                    break;
                default:
                    query = query.Where(a => !a.Canceled);
                    break;
            }

            return query;
        }

        public static IQueryable<Appointment> Order(this IQueryable<Appointment> query, string order)
        {
            switch (order.ToLower())
            {
                case "name" :
                    query = query.OrderBy(a => a.Name);
                    break;
                case "animal":
                    query = query.OrderBy(a => a.Animal.ToString());
                    break;
            }

            return query;
        }
    }
}
