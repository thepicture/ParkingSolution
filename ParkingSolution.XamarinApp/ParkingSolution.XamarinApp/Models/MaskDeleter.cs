using System.Linq;

namespace ParkingSolution.XamarinApp.Models
{
    public static class MaskDeleter
    {
        public static string DeleteMask(string input)
        {
            if (input == null) return input;
            return input
                    .Where(l =>
                    {
                        return char.IsDigit(l);
                    })
                    .Aggregate("",
                               (l1, l2) => l1 + l2);
        }
    }
}
