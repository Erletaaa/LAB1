using Lab1.Data.Models;

namespace Lab1.Data.Helpers
{
    public class ActivityHelper
    {
        private ApplicationDbContext _context;
        public ActivityHelper(ApplicationDbContext context) 
        {
            _context = context;
        }

        public Follow AddFollow(int fromUserId, int toUserId)
        {
            var follow = _context.Follows.Add(new Follow
            {
                FromUserId = fromUserId,
                ToUserId = toUserId,
                UpdatedOn = DateTime.Now
            });
            _context.SaveChanges();

            return follow.Entity;
        }

        public void RemoveFollow(int fromUserId, int toUserId)
        {
            var followInstance = _context.Follows.Where(x=>x.FromUserId==fromUserId && x.ToUserId==toUserId).FirstOrDefault(); ;

            if (followInstance == null)
                return;

            _context.Follows.Remove(followInstance);
            _context.SaveChanges();
        }

        public int CountFollowsForUser(int userId)
        {
            var followCount = _context.Follows.Where(x=>x.ToUserId==userId).Count();
            return followCount;
        }

        public Favorite AddFavorite(int productId, int userId)
        {
            var favorite = _context.Favorites.Add(new Favorite
            {
                ProductId = productId,
                UserId = userId,
                UpdatedOn = DateTime.Now
            });
            _context.SaveChanges();

            return favorite.Entity;
        }

        public void RemoveFavorite(int productId, int userId)
        {
            var favoriteInstance = _context.Favorites.Where(x => x.ProductId==productId&& x.UserId==userId).FirstOrDefault();

            if (favoriteInstance == null)
                return;

            _context.Favorites.Remove(favoriteInstance);
            _context.SaveChanges();
        }

        public int FavoritesCount(int productId)
        {
            var favoritesCount = _context.Favorites.Where(x => x.ProductId == productId).Count();
            return favoritesCount;
        }
    }
}
