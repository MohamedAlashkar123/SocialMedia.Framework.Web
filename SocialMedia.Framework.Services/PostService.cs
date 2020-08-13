using SocialMedia.Framework.Core;
using SocialMedia.Framework.Data;
using SocialMedia.Framework.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SocialMedia.Framework.Services
{
    public interface IPostService
    {
        IEnumerable<Post> GetAll();
        Post GetById(int id);
        Post Create(Post post);
        void Update(Post post);
        void Delete(int id);
    }

    public class PostService : IPostService
    {
        private SMDBContext _context;

        public PostService(SMDBContext context)
        {
            _context = context;
        }
        public IEnumerable<Post> GetAll()
        {
            return _context.Posts;
        }

        public Post GetById(int id)
        {
            return _context.Posts.Find(id);
        }

        public Post Create(Post post)
        {
            if (_context.Posts.Any(x => x.Id == post.Id))
                throw new AppException("this friendship \"" + post.Id + "\" is already taken");

            _context.Posts.Add(post);
            _context.SaveChanges();

            return post;
        }

        public void Update(Post postval)
        {
            var post = _context.Posts.Find(postval.Id);

            if (post == null)
                throw new AppException("User not found");

            _context.Posts.Update(post);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var post = _context.Posts.Find(id);
            if (post != null)
            {
                _context.Posts.Remove(post);
                _context.SaveChanges();
            }
        }
    }
}
