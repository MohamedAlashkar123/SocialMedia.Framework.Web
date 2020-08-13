using SocialMedia.Framework.Core;
using SocialMedia.Framework.Data;
using SocialMedia.Framework.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SocialMedia.Framework.Services
{
    public interface ILikeService
    {
        IEnumerable<Like> GetAll();
        Like GetById(int id);
        Like Create(Like like);
        void Update(Like like);
        void Delete(int id);
    }

    public class LikeService : ILikeService
    {
        private SMDBContext _context;

        public LikeService(SMDBContext context)
        {
            _context = context;
        }
        public IEnumerable<Like> GetAll()
        {
            return _context.Likes;
        }

        public Like GetById(int id)
        {
            return _context.Likes.Find(id);
        }

        public Like Create(Like like)
        {
            if (_context.Likes.Any(x => x.Id == like.Id))
                throw new AppException("this post is Like \"" + like.Id + "\" is already taken");

            _context.Likes.Add(like);
            _context.SaveChanges();

            return like;
        }

        public void Update(Like likeval)
        {
            var like = _context.Likes.Find(likeval.Id);

            if (like == null)
                throw new AppException("User not found");

            _context.Likes.Update(like);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var like = _context.Likes.Find(id);
            if (like != null)
            {
                _context.Likes.Remove(like);
                _context.SaveChanges();
            }
        }

    }
}
