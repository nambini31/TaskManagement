using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;
using Domain.Interface;
using Infrastructure.Data;

namespace Infrastructure.Repository
{
    public class ArticleRepository : InterfaceArticle
    {
        private readonly ApplicationDbContext _db;

        public ArticleRepository(ApplicationDbContext db)
        {
            this._db = db;
        }
        public async Task AddArticle(Article article)
        {
            
           await _db.article.AddAsync(article);
            await _db.SaveChangesAsync();

        }

        public async Task DeleteArticleById(int articleId)
        {
            var article = await _db.article.FirstOrDefaultAsync(u => u.articleId == articleId);
            
            _db.article.Remove(article);
             await _db.SaveChangesAsync();

        }

        public  async Task<Article> GetArticleById(int articleId)
        {
            Article? art =  await _db.article.FirstOrDefaultAsync(u => u.articleId == articleId);

            return art;
        }

        public async Task<IEnumerable<Article>> GetArticles()
        {
            //vrai IEnumerable<Article> data = await _db.article.Include(u => u.category).ToListAsync();
            IEnumerable<Article> data = await _db.article.ToListAsync();


            return data;
        }

        public  async Task UpdateArticle(Article article)
        {
             _db.article.Update(article);

             await _db.SaveChangesAsync();

        }
    }
}
