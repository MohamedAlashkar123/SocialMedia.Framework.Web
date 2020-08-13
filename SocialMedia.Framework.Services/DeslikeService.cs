using SocialMedia.Framework.Core;
using SocialMedia.Framework.Data;
using SocialMedia.Framework.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SocialMedia.Framework.Services
{
    public interface IDeslikeService
    {
        IEnumerable<Deslike> GetAll();
        Deslike GetById(int id);
        Deslike Create(Deslike deslike);
        void Update(Deslike deslike);
        void Delete(int id);
    }

    public class DeslikeService : IDeslikeService
    {
        private SMDBContext _context;

        public DeslikeService(SMDBContext context)
        {
            _context = context;
        }
        public IEnumerable<Deslike> GetAll()
        {
            return _context.Deslikes;
        }

        public Deslike GetById(int id)
        {
            return _context.Deslikes.Find(id);
        }

        public Deslike Create(Deslike deslike)
        {
            if (_context.Deslikes.Any(x => x.Id == deslike.Id))
                throw new AppException("this post is DesLike \"" + deslike.Id + "\" is already taken");

            _context.Deslikes.Add(deslike);
            _context.SaveChanges();

            return deslike;
        }

        public void Update(Deslike deslikeval)
        {
            var deslike = _context.Deslikes.Find(deslikeval.Id);

            if (deslike == null)
                throw new AppException("User not found");

            _context.Deslikes.Update(deslike);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var deslike = _context.Deslikes.Find(id);
            if (deslike != null)
            {
                _context.Deslikes.Remove(deslike);
                _context.SaveChanges();
            }
        }

    }
}
