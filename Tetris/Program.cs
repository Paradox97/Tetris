using System;
using System.Linq;
using System.Drawing;
using System.Timers;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;
using System.Collections;


namespace Tetris
{
    public class Field
    {
        public int width, height;

        public int block_size;

        public int start_x, start_y;

        public char[,] area;

        string player_name;

        bool exit = false;

        int period = 150;


        public Field(int width_rec, int height_rec, int startx, int starty, int blocksize, string player_name_rec) //user defined field constructor 
        {
            height = height_rec;
            width = width_rec;
            start_x = startx;
            start_y = starty;
            block_size = blocksize;
            player_name = player_name_rec;


            area = new char[width, height];

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    area[i, j] = ' '; 
                }
            }
        }

        public void render()
        {
            Console.SetCursorPosition(0, 0);

            string output = string.Empty;

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    output += area[j, i];
                }
                output += "|";
                output += "\n";
            }

            for (int j = 0; j < width; j++)
            {
                output += "^";
            }

            output += "|";

            Console.Write(output);
            
        }

        public void clear_row(Field grid, int row)
        {
                for (int j = 0; j < grid.width; j++)
                {
                    grid.area[j, row] = ' ';
                }

        }

        public void apply_gravity(Field grid, int row)
        {

            if (row - 1 < 0)
            {
                return;
            }

            for (int i = row; i > 0; i--)
            {
                for (int j = 0; j < grid.width; j++)
                {
                    grid.area[j, i] = grid.area[j, i - 1];
                    //grid.area[j, i - 1] = ' ';  // только если гравитация действует на 1 ряд
                }
            }


        }
        
        public void check_blast(Field grid)
        { 
            int row = 0;
            string row_map = String.Empty;

            for (int i = 0; i < grid.height; i++)
            {
                for (int j = 0; j < grid.width; j++)
                {
                    row_map += grid.area[j, i];
                }

                row = i;

                if (row_map.IndexOf(' ') == -1)
                {
                    clear_row(grid, row);
                    apply_gravity(grid, row);
                }

                row_map = String.Empty;
            }
        }


        public int update(ConsoleKeyInfo input, int PLAYER_TIMEOUT, Field grid, Shape figure)
        {
            input = Console.ReadKey(true);
            var Result = input;

                switch (input.Key)
                {
                    case ConsoleKey.LeftArrow:
                        figure.move_left(grid);
                        break;

                    case ConsoleKey.RightArrow:
                        figure.move_right(grid);
                        break;

                    case ConsoleKey.DownArrow:
                        figure.move_down(grid);
                        break;

                    case ConsoleKey.Spacebar:
                        figure.rotate(grid);
                        break;

                    case ConsoleKey.Escape:
                        grid.exit = true;   
                        return 1;

                    case ConsoleKey.P:
                        return 2;

                    case ConsoleKey.W:
                        if (grid.period > 10)
                            grid.period -= 10;
                        return 3;

                    case ConsoleKey.S:
                        if (grid.period < 300)
                            grid.period += 10;
                        return 4;
            }

               return 0;
        }

        void late_update() 
        { 
        
        
        
        }

        static void Main(string[] args)
        {
            const int PLAYER_TIMEOUT = 4;

            int counter = 0;

            Field grid = new Field(15, 20, 5, 0, 10, "Player1");


            Shape figure = new Shape(grid.start_x, grid.start_y);

            ConsoleKeyInfo input = Console.ReadKey(true);

            grid.render();

            while (grid.exit == false)
             {
                
                Task<int> task_update = new Task<int>(() => grid.update(input, PLAYER_TIMEOUT, grid, figure));
                task_update.Start();

                if ((counter % grid.period) == 0)
                {
                    figure.move_down(grid);
                }

                grid.check_blast(grid);
                grid.render();

                counter++;
                System.Threading.Thread.Sleep(PLAYER_TIMEOUT);
                //input = Console.ReadKey(true);
                //exit = true;


                //break;

            }

        }

    }

}