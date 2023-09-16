using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoList_web_API.Models;

namespace ToDoList_web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class todoItemsController : ControllerBase
    {
        private readonly TodoContext _context;

        public todoItemsController(TodoContext context)
        {
            _context = context;
        }

        // GET: api/todoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItemDto>>> GetTodoItems()
        {
            if (_context.TodoItems == null)
            {
                return NotFound();
            }
            else {
                // VARIANTA 1
                // List<TodoItem> todoItems = await _context.TodoItems.ToListAsync();
                // List<TodoItemDto> results = new List<TodoItemDto>();

                // foreach (TodoItem it in todoItems)
                // {
                //     results.Add(itemToDTO(it));
                // }
                // return results;
                // VARIANTA 2
                return await _context.TodoItems.Select(x => itemToDTO(x)).ToListAsync();
            }
        }

        // GET: api/todoItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItemDto>> GettodoItem(int id)
        {
          if (_context.TodoItems == null)
          {
              return NotFound();
          }
            var TodoItem = await _context.TodoItems.FindAsync(id);

            if (TodoItem == null)
            {
                return NotFound();
            }

            return Ok( itemToDTO(TodoItem) );
        }

        // PUT: api/todoItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PuttodoItem(int id, TodoItemDto todoItemDto)
        {
            if (id != todoItemDto.Id)
            {
                return BadRequest();
            }

            var todoItemSet = await _context.TodoItems.FindAsync(id);
            _context.Entry(todoItemSet).State = EntityState.Modified;

            todoItemSet.Name = todoItemDto.Name;
            todoItemSet.Id = todoItemDto.Id;
            todoItemSet.isCompleted = todoItemDto.isCompleted;

            _context.Entry(todoItemDto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!todoItemExists(id))
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

        // POST: api/todoItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TodoItem>> PosttodoItem(TodoItemDto todoItemDto)
        {
          if (_context.TodoItems == null)
          {
              return Problem("Entity set 'TodoContext.TodoItems'  is null.");
          }

            TodoItem todoItem = new TodoItem
            {
                Name = todoItemDto.Name,
                Prenom = todoItemDto.Prenom,
                Email = todoItemDto.Email,
                birthDate = todoItemDto.birthDate,
                phone = todoItemDto.phone,
                address = todoItemDto.address,
                profilePhoto = todoItemDto.profilePhoto,
                Secret = todoItemDto.Secret,
                isCompleted = todoItemDto.isCompleted,

            };
            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GettodoItem", new { id = todoItemDto.Id }, todoItemDto);
        }

        // DELETE: api/todoItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletetodoItem(int id)
        {
            if (_context.TodoItems == null)
            {
                return NotFound();
            }
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool todoItemExists(int id)
        {
            return (_context.TodoItems?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        private static TodoItemDto itemToDTO(TodoItem todoItem)
        {
            return new TodoItemDto
            {
                Id = todoItem.Id,
                Name = todoItem.Name,
                Prenom = todoItem.Prenom,
                Email = todoItem.Email,
                birthDate = todoItem.birthDate,
                phone = todoItem.phone,
                address = todoItem.address,
                profilePhoto = todoItem.profilePhoto,
                Secret = todoItem.Secret,
                isCompleted = todoItem.isCompleted,
            };
        }
    }
}
