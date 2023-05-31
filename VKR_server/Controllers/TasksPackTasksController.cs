using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VKR_server.Models;

namespace VKR_server.Controllers
{
    [EnableCors("MyPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class TasksPackTasksController : ControllerBase
    {
        private readonly PostgresContext _context;

        public TasksPackTasksController(PostgresContext context)
        {
            _context = context;
        }

        // GET: api/TasksPackTasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TasksPackTask>>> GetTasksPackTasks()
        {
          if (_context.TasksPackTasks == null)
          {
              return NotFound();
          }
            return await _context.TasksPackTasks.ToListAsync();
        }

        // GET: api/TasksPackTasks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TasksPackTask>> GetTasksPackTask(Guid id)
        {
          if (_context.TasksPackTasks == null)
          {
              return NotFound();
          }
            var tasksPackTask = await _context.TasksPackTasks.FindAsync(id);

            if (tasksPackTask == null)
            {
                return NotFound();
            }

            return tasksPackTask;
        }

        // PUT: api/TasksPackTasks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTasksPackTask(Guid id, TasksPackTask tasksPackTask)
        {
            if (id != tasksPackTask.Id)
            {
                return BadRequest();
            }

            _context.Entry(tasksPackTask).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TasksPackTaskExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TasksPackTasks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TasksPackTask>> PostTasksPackTask(TasksPackTask tasksPackTask)
        {
          if (_context.TasksPackTasks == null)
          {
              return Problem("Entity set 'PostgresContext.TasksPackTasks'  is null.");
          }
            _context.TasksPackTasks.Add(tasksPackTask);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (TasksPackTaskExists(tasksPackTask.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetTasksPackTask", new { id = tasksPackTask.Id }, tasksPackTask);
        }

        // DELETE: api/TasksPackTasks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTasksPackTask(Guid id)
        {
            if (_context.TasksPackTasks == null)
            {
                return NotFound();
            }
            var tasksPackTask = await _context.TasksPackTasks.FindAsync(id);
            if (tasksPackTask == null)
            {
                return NotFound();
            }

            _context.TasksPackTasks.Remove(tasksPackTask);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TasksPackTaskExists(Guid id)
        {
            return (_context.TasksPackTasks?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
