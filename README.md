# ToDoApp

## Запуск приложения
Для запуска приложения необходимо поменять строки подключения в файле appsettings.json на свои.
Так же для проверки фонового сервиса необходимо установить на компьютер RabbitMQ и Erlang. Репозиторий фонового сервиса https://github.com/TooSick/ToDoAppBackgroundService

## Доступные для использования методы:

### 1. **GET /api/ToDoList/GetAllToDoItems**
Возвращает список задач.

### 2. **GET /api/ToDoList/GetToDoItemById/{id}**
Возвращает задачу по заданному ID.

### 3. **POST /api/ToDoList/CreateToDoItem**
Создает новую задачу.
#### Пример тела запроса:
```json
{
  "title": "string",
  "description": "string",
  "dueDate": "2024-11-22T19:22:48.958Z",
  "priority": "Low",
  "status": "New"
}
```

### 4. **PUT /api/ToDoList/UpdateToDoItem/{id}**
Обновляет задачу по заданному ID.
#### Пример тела запроса:
```json
{
  "title": "string",
  "description": "string",
  "dueDate": "2024-11-22T19:22:48.958Z",
  "priority": "Low",
  "status": "New"
}
```

### 5. **DELETE /api/ToDoList/DeleteToDoItem/{id}**
Удаляет задачу по заданному ID.
