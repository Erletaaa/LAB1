using GarageMarketProject.Data.Models;

namespace GarageMarketProject.Data.Helpers
{
    public class ActivityHelper
    {
        private ApplicationDbContext _context;
        public ActivityHelper(ApplicationDbContext context) 
        {
            _context = context;
        }

        public void AddFollow(int fromUserId, int toUserId)
        {
            _context.Follows.Add(new Follow
            {
                FromUserId = fromUserId,
                ToUserId = toUserId,
                UpdatedOn = DateTime.Now
            });
            _context.SaveChanges();
        }

        public void RemoveFollow(int fromUserId, int toUserId)
        {
            var followInstance = _context.Follows.Where(x=>x.FromUserId==fromUserId && x.ToUserId==toUserId).FirstOrDefault(); ;

            if (followInstance == null)
                return;

            _context.Follows.Remove(followInstance);
            _context.SaveChanges();
        }

        public void AddFavorite(int productId, int userId)
        {
            _context.Favorites.Add(new Favorite
            {
                ProductId = productId,
                UserId = userId,
                UpdatedOn = DateTime.Now
            });
            _context.SaveChanges();
        }

        public void RemoveFavorite(int productId, int userId)
        {
            var favoriteInstance = _context.Favorites.Where(x => x.ProductId==productId&& x.UserId==userId).FirstOrDefault();

            if (favoriteInstance == null)
                return;

            _context.Favorites.Remove(favoriteInstance);
            _context.SaveChanges();
        }
    }
}