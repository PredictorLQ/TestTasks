using Komtek.Data;
using Komtek.Model.DateBase;
using Komtek.Model.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Komtek.Controllers
{
    /// <summary>
    /// Область работы с отчетами
    /// </summary>
    [Route("api/reports")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly DataBase _data;
        public ReportsController(DataBase _data)
        {
            this._data = _data;
        }

        /// <summary>
        /// Получите набор отчетов
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="year">Номер года</param>
        /// <param name="month">Название месяца</param>
        [HttpGet]
        [Route("{userId:int}/{year:int}/{month}")]
        public async Task<ActionResult<IEnumerable<Report>>> Get(int userId, EnumMonth month, int year)
        {
            DateTime start = new(year, (int)month, 1), end = start.AddMonths(1);
            return await _data.Report.Where(u => u.UserId == userId && u.Date >= start && u.Date < end).ToListAsync();
        }

        /// <summary>
        /// Добавьте новый отчет
        /// </summary>
        /// <param name="Report">Информация об отчете</param>
        [HttpPost]
        public async Task<ActionResult<Report>> Post(Report Report)
        {
            if (Report is null) return BadRequest();

            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (!await _data.User.AnyAsync(u => u.Id == Report.UserId)) return NotFound();

            _data.Report.Add(Report);
            await _data.SaveChangesAsync();
            return Ok(Report);
        }

        /// <summary>
        /// Измените конкретный отчет
        /// </summary>
        /// <param name="Report">Информация об отчете</param>
        [HttpPut]
        public async Task<ActionResult<Report>> Put(Report Report)
        {
            if (Report is null) return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _data.Report.AnyAsync(u => u.Id == Report.Id && u.UserId == Report.UserId)) return NotFound();

            _data.Report.Update(Report);
            await _data.SaveChangesAsync();

            return Ok(Report);
        }

        /// <summary>
        /// Удалите конкретный отчет
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     DELETE api/reports/1
        ///
        /// </remarks>
        /// <param name="id">Идентификатор отчета</param>
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            Report Report = await _data.Report.FirstOrDefaultAsync(u => u.Id == id);
            if (Report is null) return NotFound();

            _data.Report.Remove(Report);
            await _data.SaveChangesAsync();
            return Ok();
        }
    }
}
