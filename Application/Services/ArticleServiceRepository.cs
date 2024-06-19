using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;
using Domain.Interface;

namespace Application.Service
{
    public class ArticleServiceRepository
    {
        private readonly InterfaceArticle interfaceArticle;
        public ArticleServiceRepository(InterfaceArticle article) {

            this.interfaceArticle = article;

        }
        public async Task AddArticle(Article article)
        {
             await interfaceArticle.AddArticle(article);
        }

        public async Task DeleteArticleById(int articleId)
        {
           await interfaceArticle.DeleteArticleById(articleId);
        }

        public  async Task<Article> GetArticleById(int articleId)
        {
           return await interfaceArticle.GetArticleById(articleId);
        }

        public async Task<IEnumerable<Article>> GetArticles()
        {
            return  await interfaceArticle.GetArticles();
        }

        public async Task UpdateArticle(Article article)
        {
            await  interfaceArticle.UpdateArticle(article);
        }
    }
}
