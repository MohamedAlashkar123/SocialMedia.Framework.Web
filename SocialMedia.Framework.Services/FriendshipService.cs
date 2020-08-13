using SocialMedia.Framework.Core;
using SocialMedia.Framework.Data;
using SocialMedia.Framework.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SocialMedia.Framework.Services
{
    public interface IFriendshipService
    {
        IEnumerable<Friendship> GetAll();
        Friendship GetById(int id);
        Friendship Create(Friendship friendship);
        void Update(Friendship friendship);
        void Delete(int id);
    }

    public class FriendshipService : IFriendshipService
    {
        private SMDBContext _context;

        public FriendshipService(SMDBContext context)
        {
            _context = context;
        }
        public IEnumerable<Friendship> GetAll()
        {
            return _context.Friendships;
        }

        public Friendship GetById(int id)
        {
            return _context.Friendships.Find(id);
        }

        public Friendship Create(Friendship friendship)
        {
            if (_context.Friendships.Any(x => x.Id == friendship.Id))
                throw new AppException("this friendship \"" + friendship.Id + "\" is already taken");

            _context.Friendships.Add(friendship);
            _context.SaveChanges();

            return friendship;
        }

        public void Update(Friendship friendshipval)
        {
            var friendship = _context.Friendships.Find(friendshipval.Id);

            if (friendship == null)
                throw new AppException("User not found");

            _context.Friendships.Update(friendship);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var friendship = _context.Friendships.Find(id);
            if (friendship != null)
            {
                _context.Friendships.Remove(friendship);
                _context.SaveChanges();
            }
        }
    }
}