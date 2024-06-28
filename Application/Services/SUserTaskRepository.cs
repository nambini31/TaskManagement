
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
using Microsoft.AspNetCore.Mvc;

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
        public async Task AddUserTask(List<UserTask> article)
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
       
        public async Task UpdateHistoGenereExcel(Export export)
        {
            await IUserTask.UpdateHistoGenereExcel(export);
        } 
        
        public async Task<string> GenerateUserTask(FiltreUserTask filter)
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

                    worksheet.Cells[2, 1].Value = $"From : { filter.startDate.ToString("yyyy-MM-dd")}  To : {filter.endDate.ToString("yyyy-MM-dd")}";

                    worksheet.Cells["A4"].Value = "Tasks for project";

                    
                    int column = 2;
                    foreach (var userName in users)
                    {
                        worksheet.Cells[4, column].Value = $"{userName.userName}";
                        column++;
                    }

                    worksheet.Cells[4, column].Value = "Total";

                   

                    int row = 5; 
                    foreach (var taskName in tasks)
                    {
                        worksheet.Cells[$"A{row}"].Value = taskName.taskName; 

                        column = 2;
                        
                        double total = 0.0;

                        foreach (var user in users)
                        {
                             var entete = worksheet.Cells[4, column].Value;

                            if (entete != null)
                            {
                                foreach (var datas in data)
                                {


                                    if (datas.userName == user.userName && taskName.taskName == datas.taskName && entete.ToString() == user.userName)
                                    {
                                        worksheet.Cells[row, column].Value = datas.hours;
                                        total += datas.hours;
                                    }

                                }
                            }

                            column++;
                        }


                        worksheet.Cells[row, column].Value = total;


                        row++;
                    }

                    using (var range = worksheet.Cells[$"A4:{Convert.ToChar('A' + users.Count()+1)}4"])
                    {
                        range.Style.Font.Bold = true;
                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGreen);
                    }

                    package.SaveAs(new FileInfo(filePath));
                }

                

                return filePath;
                
            }
            catch (Exception ex)
            {
                // Gérer l'erreur (journalisation, traitement, etc.)
                // Ne pas utiliser throw ex; pour ne pas perdre la stack trace
                throw;
            }

        }
    }
}
