﻿using Application.TodoLists.Queries.ExportTodos;
using System.Collections.Generic;

namespace Application.Common.Interfaces
{
    public interface ICsvFileBuilder
    {
        byte[] BuildTodoItemsFile(IEnumerable<TodoItemRecord> records);
    }
}
