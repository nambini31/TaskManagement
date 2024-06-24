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

        public async Task DeleteUserTaskById(int articleId)
        {
           await IUserTask.DeleteUserTaskById(articleId);
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

                    int column = 2; // Start adding additional headers from column C

                    foreach (var item in users)
                    {
                        worksheet.Cells[1, column].Value = $"{item.userName}";
                        column++;
                    }

                    // Adding data
                    int row = 2; // Start from row 2 after headers
                    foreach (var task in data)
                    {
                        worksheet.Cells[$"A{row}"].Value = task.taskName; // Task name

                        // Initialize hours for each user to zero
                        Dictionary<int, double> userHours = new Dictionary<int, double>();
                        foreach (var user in users)
                        {
                            userHours[user.userId] = 0.0;
                        }

                        // Assign hours based on user and task
                        foreach (var hour in data.Where(d => d.taskName == task.taskName))
                        {
                            userHours[hour.userId] += hour.hours;
                        }

                        // Fill in the hours for each user
                        column = 2; // Start from column B
                        foreach (var user in users)
                        {
                            worksheet.Cells[row, column].Value = userHours[user.userId];
                            column++;
                        }

                        row++;
                    }

                    // Formatting header
                    using (var range = worksheet.Cells["A1:J1"])
                    {
                        range.Style.Font.Bold = true;
                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                    }

                    // Save to the specified path
                    package.SaveAs(new FileInfo(filePath));
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            

            //return Ok(new { message = "File saved successfully", filePath });
        }
    }
}
