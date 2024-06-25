using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.DTO.ViewModels;
using Domain.Entity;
using Domain.Interface;
using OfficeOpenXml.Style;
using Microsoft.AspNetCore.Hosting;
using OfficeOpenXml;

namespace Application.Services
{
    public class SUserTaskRepository
    {
        private readonly IUserTaskRepository IUserTask;
        private readonly IHostingEnvironment _env;
        public SUserTaskRepository(IUserTaskRepository IUserTask , IHostingEnvironment _env) {

            this.IUserTask = IUserTask;
            this._env = _env;

        }
        public async Task AddUserTask(UserTask article)
        {
             await IUserTask.AddUserTask(article);
        }

        public async Task DeleteUserTaskById(int articleId, int userConnected)
        {
           await IUserTask.DeleteUserTaskById(articleId, userConnected);
        }

        public  async Task<UserTask> GetUserTaskById(int articleId)
        {
           return await IUserTask.GetUserTaskById(articleId);
        }

        public async Task<IEnumerable<UserTask>> GetUserTask()
        {
            return  await IUserTask.GetUserTask();
        } 
        public async Task<IEnumerable<UserTaskVM>> GetUserTaskVM(FiltreUserTask filter)
        {
            return await IUserTask.GetUserTasksVM(filter);
        }

        public async Task UpdateUserTask(UserTask userTask)
        {
            await IUserTask.UpdateUserTask(userTask);
        } 
        
        public async Task GenerateUserTask(FiltreUserTask filter)
        {

            try
            {
                IEnumerable<UserTaskVM> users = await IUserTask.GetUserTasksForTwoDate(filter);
                IEnumerable<UserTaskVM> data = await IUserTask.GetUserTasksByUsersVM(filter);
                IEnumerable<UserTaskVM> tasks = await IUserTask.GetUserTasksGrouperVM(filter);

                var uploadsFolderPath = Path.Combine(_env.WebRootPath, "assets", "uploads");
                if (!Directory.Exists(uploadsFolderPath))
                {
                    Directory.CreateDirectory(uploadsFolderPath);
                }

                var fileName = $"UserTask_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                var filePath = Path.Combine(uploadsFolderPath, fileName);

                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("User Task");

                    // Adding headers
                    worksheet.Cells["A1"].Value = "Tasks for project";

                     // Commence à la ligne 2 après les en-têtes , lire l'entete
                    int column = 2;
                    foreach (var userName in users)
                    {
                        worksheet.Cells[1, column].Value = $"{userName.userName}";
                        column++;
                    }

                    // Ajouter les données dans la feuille Excel
                    int row = 2; // Commencer à partir de la ligne 2 après les en-têtes
                    foreach (var taskName in tasks)
                    {
                        worksheet.Cells[$"A{row}"].Value = taskName.taskName; // Nom de la tâche

                        // Remplir les heures pour chaque utilisateur
                        column = 2; // Commencer à partir de la colonne B après la première colonne "Tasks for project"
                        foreach (var datas in data)
                        {

                           var entete = worksheet.Cells[1, column].Value;

                            if (entete == datas.userName)
                            {
                                
                            }
                            //var userTask = data.FirstOrDefault(d => d.taskName == taskName && d.userName == userName);
                            //if (userTask != null)
                            //{
                            //    worksheet.Cells[row, column].Value = userTask.hours;
                            //}
                            column++;
                        }

                        row++;
                    }

                    // Formater l'en-tête
                    using (var range = worksheet.Cells[$"A1:{Convert.ToChar('A' + users.Count())}1"])
                    {
                        range.Style.Font.Bold = true;
                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                    }

                    // Save to the specified path
                    package.SaveAs(new FileInfo(filePath));
                }

                // Retourner le fichier Excel en tant que téléchargement
               // byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
                //return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
            catch (Exception ex)
            {
                // Gérer l'erreur (journalisation, traitement, etc.)
                // Ne pas utiliser throw ex; pour ne pas perdre la stack trace
                throw;
            }




            //return Ok(new { message = "File saved successfully", filePath });
        }
    }
}
