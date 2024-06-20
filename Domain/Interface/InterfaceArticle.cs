using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;
using Task = System.Threading.Tasks.Task;

namespace Domain.Interface
{
    public interface InterfaceArticle
    {
        Task<IEnumerable<Article>> GetArticles();
        Task<Article> GetArticleById(int articleId);
        Task AddArticle(Article article);
        Task UpdateArticle(Article article);
        Task DeleteArticleById(int articleId);

    }
}
