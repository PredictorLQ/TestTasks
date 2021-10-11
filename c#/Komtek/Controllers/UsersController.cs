using Komtek.Data;
using Komtek.Model.DateBase;
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
    /// Область работы с пользователями
    /// </summary>
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DataBase _data;
        public UsersController(DataBase _data)
        {
            this._data = _data;
        }
        /// <summary>
        /// Получите набор пользователей
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> Get() => await _data.User.AsNoTracking().ToListAsync();

        /// <summary>
        /// Добавьте нового пользователя
        /// </summary>
        /// <param name="User">Информация о пользователе</param>
        [HttpPost]
        public async Task<ActionResult<User>> Post(User User)
        {
            if (User is null) return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (await _data.User.AnyAsync(u => u.Mail == User.Mail))
            {
                ModelState.AddModelError("mail", "user is exists!");
                return BadRequest(ModelState);
            }

            _data.User.Add(User);
            await _data.SaveChangesAsync();
            return Ok(User);
        }

        /// <summary>
        /// Измените информацию об отдельном пользователе
        /// </summary>
        /// <param name="User">Информация о пользователе</param>
        [HttpPut]
        public async Task<ActionResult<User>> Put(User User)
        {
            if (User is null) return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _data.User.AnyAsync(u => u.Id == User.Id)) return NotFound();

            _data.User.Update(User);
            await _data.SaveChangesAsync();
            return Ok(User);
        }
        /// <summary>
        /// Удалите пользователя
        /// </summary>
        /// <remarks>
        /// Пример запроса:
        ///
        ///     DELETE api/users/1
        ///
        /// </remarks>
        /// <param name="id">Идентификатор пользователя</param>
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            User User = await _data.User.FirstOrDefaultAsync(u => u.Id == id);
            if (User is null) return NotFound();

            _data.User.Remove(User);
            await _data.SaveChangesAsync();
            return Ok();
        }
    }
}
