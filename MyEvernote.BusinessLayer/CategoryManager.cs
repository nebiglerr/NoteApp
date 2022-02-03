using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyEvernote.BusinessLayer.Abstract;
using MyEvernote.Entities;


namespace MyEvernote.BusinessLayer
{
    public class CategoryManager : ManagerBase<Category>
    {
        public override int Delete(Category category)
        {
            NoteManager noteManager = new NoteManager();
            LikedManager likedManager = new LikedManager();
            CommentManager commentManager = new CommentManager();

            // Kategori ile ilişkili notların silinmesi
            foreach (Note note in category.Notes.ToList())
            {
                //note ile ilişkili likelerin silinmesi
                foreach (Liked like in note.Likes.ToList())
                {
                    likedManager.Delete(like);
                }

                // note ile comment(yorum) ilişkisi olanları sil
                foreach (Comment comment in note.Comments.ToList())
                {
                    commentManager.Delete(comment);
                }
                noteManager.Delete(note);
            }
            return base.Delete(category);
        }
        

    }
}
