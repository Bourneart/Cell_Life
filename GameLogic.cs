using System;

namespace CreateLife2
{
    public class GameLogic
    {
        private bool[,] field;
        private readonly int rows;
        private readonly int cols;
        public uint iterations { get; private set; }

        public GameLogic(int rows, int cols, int density)
        {
            this.rows = rows;
            this.cols = cols;
            field = new bool[cols, rows];
            Random random = new Random();
            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    field[x, y] = random.Next(density) == 0;
                }
            }
        }
        public void NextGeneration() //Генерирование следующего поколения клеток
        {
            var newField = new bool[cols, rows];

            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    var neighborsCount = CountofNeighbors(x, y);
                    var isLife = field[x, y];

                    if (!isLife && neighborsCount == 3)
                    {
                        newField[x, y] = true;
                    }
                    else if (isLife && (neighborsCount < 2 || neighborsCount > 3))
                    {
                        newField[x, y] = false;
                    }
                    else { newField[x, y] = field[x, y]; }
                }
            }
            field = newField;
            iterations++;
        }

        public bool[,] GetCurrentGeneration() //Получение текущего положения клеток и копирование в новый массив
        {
            var result = new bool[cols, rows];
            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    result[x, y] = field[x, y];
                }
            }
            return result;
        }
        private int CountofNeighbors(int x, int y) //Метод подсчета количество соседей у клетки
        {
            int count = 0;
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    var col = (x + i + cols) % cols;
                    var row = (y + j + rows) % rows;

                    var isSelfChecking = col == x && row == y;
                    var isLife = field[col, row];

                    if (isLife && !isSelfChecking) { count++; }
                }
            }
            return count;
        }

        private bool ValidCellPosition(int x, int y) //Валидация позиции клетки, для исключения выхода за пределы игрового поля
        {
            return x >= 0 && y >= 0 && x < cols && y < rows;
        }

        private void UpdateCell (int x, int y, bool state) //Обновление состояния клеток для работы метода Add и Delete
        {
            if (ValidCellPosition(x, y))
                field[x, y] = state;
        }

        public void AddCell(int x, int y) //Добавление клеток
        {
            UpdateCell(x, y, state: true);
        }

        public void DeleteCell(int x, int y) //Удаление клеток
        {
            UpdateCell(x, y, state: false);
        }
    }
}
