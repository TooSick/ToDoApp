Для запуска приложения необходимо поменять строки подключения в файле appsettings.json на свои.
Доступные для использования методы:
  GET /api/ToDoList/GetAllToDoItems
  GET /api/ToDoList/GetToDoItemById/{id}
  POST /api/ToDoList/CreateToDoItem
  {
    "title": "string",
    "description": "string",
    "dueDate": "2024-11-22T19:22:48.958Z",
    "priority": Low,
    "status": New
  }
  PUT /api/ToDoList/UpdateToDoItem/{id}
  {
    "title": "string",
    "description": "string",
    "dueDate": "2024-11-22T19:22:48.958Z",
    "priority": Low,
    "status": New
  }
  DELETE /api/ToDoList/DeleteToDoItem/{id}
