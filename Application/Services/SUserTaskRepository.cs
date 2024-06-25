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

                    
                    worksheet.Cells["A1"].Value = "Tasks for project";

                    
                    int column = 2;
                    foreach (var userName in users)
                    {
                        worksheet.Cells[1, column].Value = $"{userName.userName}";
                        column++;
                    }

                    
                    int row = 2; 
                    foreach (var taskName in tasks)
                    {
                        worksheet.Cells[$"A{row}"].Value = taskName.taskName; 

                        column = 2; 

                        foreach (var user in users)
                        {
                             var entete = worksheet.Cells[1, column].Value;

                            if (entete != null)
                            {
                                foreach (var datas in data)
                                {


                                    if (datas.userName == user.userName && taskName.taskName == datas.taskName && entete.ToString() == user.userName)
                                    {
                                        worksheet.Cells[row, column].Value = $"{datas.hours}";
                                    }

                                }
                            }

                            column++;
                        }


                        

                        row++;
                    }

                    using (var range = worksheet.Cells[$"A1:{Convert.ToChar('A' + users.Count())}1"])
                    {
                        range.Style.Font.Bold = true;
                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                    }

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
